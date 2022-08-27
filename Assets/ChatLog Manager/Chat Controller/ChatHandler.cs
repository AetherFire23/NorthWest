using Assets;
using Assets.ChatLog_Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Cysharp.Threading.Tasks;
using Assets.ChatLog_Manager.Chat_Controller;
using Assets.ChatLog_Manager.Private_Rooms.ChatModels;
using Assets.GameState_Management.Models;
using Assets.GameState_Management;

public partial class ChatHandler : ITickable, IInitializable
{
    public List<TextObjectInstance> _messagesInstances { get; set; } = new();
    public Guid CurrentRoomId { get; set; } // initialized through room tabs 

    private readonly MainPlayer _mainPlayer;
    private readonly ChatWrapper _ChatObject;
    private readonly MessageService _messageService;
    private readonly GameStateManager _gameStateManager;
    private readonly InvitePanel _invitePanel;
    private SimpleTimer _timer = new SimpleTimer(3);

    private List<MessageModel> LoadedMessages { get; set; } = new();
    private bool HasNewMessage => _gameStateManager.NewMessages.Any(); // Event pour ne pas check tous les frames ?
    private Guid GlobalGameId => _mainPlayer.GameId;

    public ChatHandler( 
        MainPlayer mainPlayer,
        ChatWrapper chatObject,
        MessageService messageService,
        GameStateManager gameState,
        InvitePanel invitePanel)
    {
        _ChatObject = chatObject;
        _mainPlayer = mainPlayer;
        _messageService = messageService;
        _gameStateManager = gameState;
        _invitePanel = invitePanel;
    }

    public void Initialize()
    {
        this.CurrentRoomId = GlobalGameId;

        var playersInGlobal = _gameStateManager.Players;
        _invitePanel.InitializePlayerEntries(playersInGlobal);
    }

    public void Tick() // faudra implementer du scrolling. 
    {
        bool userHasPressedEnter = Input.GetKeyDown(KeyCode.Return);
        if (userHasPressedEnter)
        {
            MessageResult messageResult = GetInputMessageResult();
            if (messageResult.IsEmpty) return;

            _messageService.SendMessageToDatabase(messageResult.Message, this.CurrentRoomId);
            _ChatObject.ClearInputField();
        }

        LoadNewMessages();
        _timer.AddTime(Time.deltaTime);
    }

    private void LoadNewMessages()
    {
        if (HasNewMessage)
        {
            var onlyNewMessages = RemoveExistingMessages(LoadedMessages, _gameStateManager.NewMessages);
            this.LoadedMessages.AddRange(onlyNewMessages);
            CreateMessageInstances(onlyNewMessages);
        }
    }

    private List<MessageModel> RemoveExistingMessages(List<MessageModel> existingMessages, List<MessageModel> newMessages)
    {
        var oldMessagesIds = existingMessages.Select(mess => mess.Id);
        var newMessagesOnly = newMessages.Where(mess => !oldMessagesIds.Contains(mess.Id)).ToList();
        return newMessagesOnly;
    }

    private void PostMessagesInRoom(Guid roomId)
    {
        var roomMessages = LoadedMessages.Where(mess => mess.RoomId == roomId).ToList();
        CreateMessageInstances(roomMessages);
    }

    private void CreateMessageInstances(List<MessageModel> messages)
    {
        foreach (MessageModel message in messages)
        {
            var newInstance = new TextObjectInstance(_ChatObject.ChatBehaviour.ContentObject, message);
            this._messagesInstances.Add(newInstance);
        }
    }

    private void ClearChatlogC()
    {
        _messagesInstances.ForEach(message => message.UnityInstance.SelfDestroy());
        _messagesInstances.Clear();
    }
}