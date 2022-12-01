using Assets.ChatLog_Manager;
using Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem;
using Assets.GameState_Management;
using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using System;
using Assets.Big_Tick_Energy;
using WebAPI.Models;
using Cysharp.Threading.Tasks;

public class ChatManager : MonoBehaviour
{
    //Setup - Dependencies
    [Inject] private ChatObjectsManager _chatObjectsManager;
    [Inject] private GameStateManager _gamestate;
    [Inject] private GlobalTick _tick;
    [Inject] private ClientCalls _client;

    private UGICollectionEditorDbKey<TextObjectUGI, TextObjectScript, Message> _messages; // Messages have a dbModel
    private UGICollectionEditorDbKey<PlayerInRoomEntryUGI, PlayerInRoomEntryAS, Player> _playersInRoom;
    //private UGICollectionEditor<RoomTabUGI, RoomTabInstanceScript> _roomTabs;
    private UGICollectionEditor<RoomTabUGI, RoomTabInstanceScript> _roomTabs;

    private UGICollectionEditor<InviteButtonUGI, InviteButtonInstanceScript> _inviteButtons;

    // Real variables
    private Guid _currentChatRoomId;
    private List<Message> MessageBank = new();


    void Start()
    {
        _tick.TimerTicked += this.OnTimerTick;
        this.MessageBank.AddRange(_gamestate.NewMessages);

        // Define how the buttons works
        _chatObjectsManager.GlobalButton.AddMethod(() => this.ReturnToGlobalChatRoom());
        _chatObjectsManager.AddRoomButton.AddMethod(() => this.CreateNewPrivateRoom());
        _chatObjectsManager.LeaveRoomButton.AddMethod(RemoveCurrentPrivateRoom);
        _chatObjectsManager.OpenInvitePanel.AddMethod(() => _chatObjectsManager.enabled = !_chatObjectsManager.enabled);

        // Variables
        _currentChatRoomId = _gamestate.LocalPlayerDTO.GameId;

        // Define how the collections should build their UGIs
        _playersInRoom = new((playerModel) => new PlayerInRoomEntryUGI(this._chatObjectsManager.PlayerInRoomContainer, playerModel));
        _messages = new((message) => new TextObjectUGI(_chatObjectsManager.ScrollviewContent, message));
        _roomTabs = new();
        _inviteButtons = new();

        // Run some stuff after dependencies are dealt with
        RefreshPlayersInRoom();
        RefreshMessages();
        RefreshPrivateRoomTabs();
        LoadPrivateInvitationButtons();
    }

    private void RefreshPlayersInRoom()
    {
        var playersInRoom = new List<Player>();

        if (_currentChatRoomId.Equals(_gamestate.LocalPlayerDTO.GameId))
        {
            playersInRoom = _gamestate.Players;
        }

        else
        {
            playersInRoom = _gamestate.ParticipantsInPlayerRooms.Where(x => _currentChatRoomId.Equals(x.RoomId))
                .Select(x => x.ParticipantId)
                .Join(_gamestate.Players,
                (id) => id,
                (p) => p.Id,
                (id, p) => p).ToList();
        }
        _playersInRoom.RefreshFromDbModels(playersInRoom);
    }

    private void RefreshMessages()
    {
        var messagesInCurrentRoom = this.MessageBank.Where(x => x.RoomId.Equals(_currentChatRoomId)).ToList();
        _messages.RefreshFromDbModels(messagesInCurrentRoom);
    }
    private void RefreshPrivateRoomTabs()
    {
        var privateRoomIds = _gamestate.ParticipantsInPlayerRooms.Select(x => x.RoomId).Distinct().ToList();
        var currentRoomIds = _roomTabs.UGIs.Select(x => x.RoomId).ToList();

        var appearedRooms = privateRoomIds.Where(x => !currentRoomIds.Contains(x)).ToList();
        var disappeared = currentRoomIds.Where(x => !privateRoomIds.Contains(x)).ToList();

        foreach (Guid guid in appearedRooms)
        {
            var tab = _roomTabs.Add(new RoomTabUGI(_chatObjectsManager.TabsContainer, guid, this._roomTabs.UGIs.Count + 1));
            tab.ButtonComponent.AddMethod(() =>
            {
                _currentChatRoomId = tab.RoomId;
                this.RefreshMessages();
                this.RefreshPlayersInRoom();
            });
        }

        foreach (var guid in disappeared)
        {
            var ugisToRemove = _roomTabs.UGIs.Where(x => disappeared.Contains(x.RoomId)).ToList();
            _roomTabs.RemoveMany(ugisToRemove);
        }
    }

    // Through addMethod
    private void ReturnToGlobalChatRoom()
    {
        _currentChatRoomId = _gamestate.LocalPlayerDTO.GameId;
        this.RefreshMessages();
        this.RefreshPlayersInRoom();
    }

    private void CreateNewPrivateRoom() // +
    {
        Guid guid = Guid.NewGuid();
        var s = _client.Chat.CreateChatroom(guid).AsTask().Result;
        var tab = _roomTabs.Add(new RoomTabUGI(_chatObjectsManager.TabsContainer, guid, _roomTabs.UGIs.Count + 1));
        tab.ButtonComponent.AddMethod(() =>
        {
            _currentChatRoomId = tab.RoomId;
            this.RefreshMessages();
            this.RefreshPlayersInRoom();
        });
    }

    private void RemoveCurrentPrivateRoom() // -
    {
        var tabToRemove = _roomTabs.UGIs.First(x => x.RoomId == _currentChatRoomId);
        _client.Chat.LeaveChatRoom(tabToRemove.RoomId);
        ReturnToGlobalChatRoom();
        _roomTabs.RemoveAndDestroy(tabToRemove);

        // reset les noms
        for (int i = 0; i < _roomTabs.UGIs.Count; i++)
        {
            var s = _roomTabs.UGIs.ElementAt(i);
            s.AccessScript.buttonText.text = $"Room {i}";
        }

        // remove room in http
    }

    private void LoadPrivateInvitationButtons() // ds le invite panel
    {
        foreach (var p in _gamestate.Players)
        {
            // le id du targetPlayer est contenu dans le bouton
            var s = _inviteButtons.Add(new InviteButtonUGI(_chatObjectsManager.InvitePanel, p));
            s.AccessScript.Button.AddMethod(() => SendPrivateInvitation(p.Id));
        }
    }

    private void SendPrivateInvitation(Guid targetId)
    {
        if (_currentChatRoomId == _gamestate.LocalPlayerDTO.GameId) return;

        _client.Chat.InviteToRoom(targetId);
    }


    private void OnTimerTick(object source, EventArgs e)
    {
        _tick.SubscribedMembers.Add(this.GetType().Name);
        this.MessageBank.AddRange(_gamestate.NewMessages);
        this.RefreshMessages();
        this.RefreshPrivateRoomTabs();
    }
}