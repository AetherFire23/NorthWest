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
    private Guid _currentChatRoom;
    private List<Message> MessageBank = new();


    void Start()
    {
        _tick.TimerTicked += this.OnTimerTick;
        this.MessageBank.AddRange(_gamestate.NewMessages);

        // Define how the buttons works
        _chatObjectsManager.GlobalButton.AddMethod(()=> this.ReturnToGlobalChatRoom());
        _chatObjectsManager.AddRoomButton.AddMethod(()=> this.CreateNewPrivateRoom());
        // Variables
        _currentChatRoom = _gamestate.LocalPlayerDTO.GameId;

        // Define how the collections should build their UGIs
        _playersInRoom = new((playerModel) => new PlayerInRoomEntryUGI(this._chatObjectsManager.PlayerInRoomContainer, playerModel));
        _messages = new((message) => new TextObjectUGI(_chatObjectsManager.ScrollviewContent, message));
        _roomTabs = new();

        // Run some stuff after dependencies are dealt with
        RefreshPlayersInRoom();
        RefreshMessages();
        RefreshPrivateRoomTabs();


        // RefreshRoomTabs();
        // setup :
        // Load Players in room
        // Load Messages
        // Load Private rooms
        // Load roomTabs + make the buttons usable
    }

    private void RefreshPlayersInRoom()
    {
        var playersInRoom = new List<Player>();
        // Gets players in room
        if (_currentChatRoom.Equals(_gamestate.LocalPlayerDTO.GameId))
        {
            playersInRoom = _gamestate.Players;
        }

        else
        {
            playersInRoom = _gamestate.ParticipantsInPlayerRooms.Where(x => _currentChatRoom.Equals(x.RoomId))
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
        var messagesInCurrentRoom = this.MessageBank.Where(x=> x.RoomId.Equals(_currentChatRoom)).ToList();
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
            _roomTabs.Add(new RoomTabUGI(_chatObjectsManager.TabsContainer, guid, this._roomTabs.UGIs.Count + 1));
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
        _currentChatRoom = _gamestate.LocalPlayerDTO.GameId;
        this.RefreshMessages();
        this.RefreshPlayersInRoom();
    }

    private void CreateNewPrivateRoom() // +
    {
        // var s = _client.AddPlayerRoomPair().AsTask().Result;
        var s = _client.AddPlayerRoomPair(Guid.NewGuid());
    }

    private void RemoveCurrentPrivateRoom() // -
    {

    }

    private void LoadPrivateInvitationButtons() // ds le invite panel
    {

    }
    private void OnTimerTick(object source, EventArgs e)
    {
        _tick.SubscribedMembers.Add(this.GetType().Name);
        this.MessageBank.AddRange(_gamestate.NewMessages);
        this.RefreshMessages();
        this.RefreshPrivateRoomTabs();
    }
}