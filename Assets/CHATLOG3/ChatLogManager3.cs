
using Assets;
using Assets.AssetLoading;
using Assets.CHATLOG3;
using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.FACTOR3;
using Assets.GameLaunch;
using Assets.GameState_Management;
using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class ChatLogManager3 : MonoBehaviour, IStartupBehavior, IRefreshable
{
    [SerializeField] ChatObjectsManager _chatObjects;
    [SerializeField] PrefabLoader _prefabLoader;
    [SerializeField] Calls _calls;
    [SerializeField] DialogManager _dialogManager;

    private List<Message> _allMessages = new();
    private GameObjectDatabaseRefresher<ChatTextObject, Message> _chatTexts; // text in chat
    private GameObjectDatabaseRefresher<PlayerInRoomButton, Player> _playersInChatRoom;
    private GameObjectDatabaseRefresher<RoomTab, PrivateChatRoom> _roomTabs;
    private List<InviteButton> _inviteButtons = new();

    private GameState _gameState;
    private Guid _currentShownRoomId;
    private Guid _playerUID => _gameState.PlayerUID;

    public async UniTask Initialize(GameState firstGameState)
    {
        _gameState = firstGameState;
        _currentShownRoomId = _gameState.GameId;
        _allMessages.AddRange(_gameState.NewMessages);
        await InitializeGameObjectUpdaters();
        await InitializeStaticButtons();
        await InitializeInviteButtons(_gameState.Players);
        await InitializeAddChatRoomButton();
        await RefreshPanelsForChatRoom(_currentShownRoomId);
    }

    public async UniTask Refresh(GameState gameState)
    {
        _gameState = gameState;
        _allMessages.AddRange(_gameState.NewMessages);
        await RefreshPanelsForChatRoom(_currentShownRoomId);
    }

    public async UniTask RefreshPanelsForChatRoom(Guid chatRoomId)
    {
        await LoadMessagesInChatRoom(chatRoomId);
        await RefreshChatRoomParticipantsInChatRoom(chatRoomId);
        await RefreshRoomTabs();
    }

    public async UniTask LoadMessagesInChatRoom(Guid chatRoomId)
    {
        var messagesInChatRoom = _allMessages.Where(x => x.RoomId.Equals(chatRoomId)).ToList();
        await _chatTexts.RefreshEntities(messagesInChatRoom);
    }

    public async UniTask RefreshChatRoomParticipantsInChatRoom(Guid roomId)
    {
        Debug.Log("Refre");
        _currentShownRoomId = roomId;
        var players = await _gameState.GetPlayersInChatRoom(_currentShownRoomId);
        await _playersInChatRoom.RefreshEntities(players);
    }

    public async UniTask RefreshRoomTabs()
    {
        await _roomTabs.RefreshEntities(_gameState.PrivateChatRooms);
    }

    // --- Initialization Methods ---
    public async UniTask InitializeGameObjectUpdaters() // runnable on threadpool ?
    {
        _chatTexts = new(CreateChatText);
        _playersInChatRoom = new(CreatePlayerInRoomButton);
        _roomTabs = new(CreateRoomTab);
    }

    public async UniTask InitializeInviteButtons(List<Player> playersInGame)
    {
        foreach (var player in playersInGame)
        {
            RoomInvitationParameters parameters = new RoomInvitationParameters()
            {
                PlayerName = player.Name,
                FromId = _playerUID,
                TargetPlayer = player.Id,
                TargetRoomId = _currentShownRoomId
            };
            await CreateInviteButton(parameters);
        }
    }

    public async UniTask InitializeAddChatRoomButton()
    {
        // creates the local ID locally to be able to immediately refresh to it.
        var guid = Guid.NewGuid();
        Func<UniTask> createDelegate = async () =>
        {
            // if you want to have an input field for the room name, do it here 
            var response = await _calls.CreateChatroom(_playerUID, Guid.NewGuid());
            await this.CreateRoomTab(new PrivateChatRoom()
            {
                Id = guid,
                ChatRoomName = "newRoom",
            });
        };
        _chatObjects.AddRoomButton.AddTaskFunc(createDelegate);
    }

    public async UniTask<ChatTextObject> CreateChatText(Message message)
    {
        var prefab = await _prefabLoader.CreateInstanceOfAsync<ChatTextObject>(_chatObjects.ScrollviewContent);
        await prefab.Initialize(message);
        return prefab;
    }

    public async UniTask<PlayerInRoomButton> CreatePlayerInRoomButton(Player player)
    {
        var prefab = await _prefabLoader.CreateInstanceOfAsync<PlayerInRoomButton>(_chatObjects.PlayerInRoomContainer);
        await prefab.Initialize(player);
        return prefab;
    }

    public async UniTask<RoomTab> CreateRoomTab(PrivateChatRoom chatRoom) // will have to create PrivateChatRoom to WebAPI
    {
        var prefab = await _prefabLoader.CreateInstanceOfAsync<RoomTab>(_chatObjects.TabsContainer);

        await prefab.Initialize(chatRoom, async () =>
        {
            _currentShownRoomId = chatRoom.Id;
            await RefreshPanelsForChatRoom(chatRoom.Id);

        });
        return prefab;
    }

    public async UniTask CreateInviteButton(RoomInvitationParameters parameters)
    {
        var inviteButton = await _prefabLoader.CreateInstanceOfAsync<InviteButton>(_chatObjects.InvitePanel);

        Func<UniTask> invitePerson = async () =>
        {
            var response = await _calls.InviteToRoom(parameters);
            if (response.IsSuccessful) return;

            var messageBox = await _dialogManager.CreateDialog<MessageBox>();
            // The code to check for duplicate invitation does not seem to work / exist. 
            await messageBox.Initialize(response.Message);
            await messageBox.WaitForResolveCoroutine();
            await messageBox.Destroy();
        };

        await inviteButton.Initialize(parameters.PlayerName, invitePerson);
        _inviteButtons.Add(inviteButton);
    }

    public async UniTask InitializeStaticButtons()
    {
        _chatObjects.GlobalButton.AddTaskFunc(async () => await RefreshPanelsForChatRoom(_gameState.GameId));
    }

    public Guid GetCurrentShownRoom()
    {
        return _currentShownRoomId;
    }
}
