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
using Assets.GameState_Management;
using Assets.Big_Tick_Energy;

public partial class ChatHandler : ITickable, IInitializable
{
    public List<TextObjectUGI> _messagesInstances { get; set; } = new();
    public Guid CurrentRoomId { get; set; } // initialized through room tabs 

    private readonly GlobalTick _globalTick;
    private readonly ChatUGF _ChatObject;
    private readonly MessageService _messageService;
    private readonly GameStateManager _gameStateManager;
    private readonly PlayerInRoomPanelUGF _invitePanel;
    private SimpleTimer _timer = new SimpleTimer(3);

    private List<Message> LoadedMessages { get; set; } = new();
    private bool HasNewMessage => _gameStateManager.NewMessages.Any();
    private Guid GlobalGameId => _gameStateManager.LocalPlayerDTO.GameId;

    public ChatHandler( 
        ChatUGF chatObject,
        MessageService messageService,
        GameStateManager gameState,
        PlayerInRoomPanelUGF invitePanel,
        GlobalTick globalTick)
    { 
        _globalTick = globalTick;
        _ChatObject = chatObject;
        _messageService = messageService;
        _gameStateManager = gameState;
        _invitePanel = invitePanel;
    }

    public void Initialize()
    {
        this.CurrentRoomId = GlobalGameId; 
        var playersInGlobal = _gameStateManager.Players;
        _invitePanel.InitializePlayerEntries(playersInGlobal);

        CreateMessageInstances(_gameStateManager.NewMessages); // initialiser les messages ici pour pas que le gamestate <oublie> les messages au 1er tick.
        _globalTick.TimerTicked += this.OnTimerTick;
    }

    public void Tick() // faudra implementer du scrolling. 
    {
        //bool userHasPressedEnter = Input.GetKeyDown(KeyCode.Return);
        //if (userHasPressedEnter)
        //{
        //    MessageResult messageResult = GetInputMessageResult();
        //    if (messageResult.IsEmpty) return;

        //    _messageService.SendMessageToDatabase(messageResult.Message, this.CurrentRoomId);
        //    _ChatObject.ClearInputField();
        //}

        _timer.AddTime(Time.deltaTime);
    }

    private List<Message> RemoveExistingMessages(List<Message> existingMessages, List<Message> newMessages)
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

    private void CreateMessageInstances(List<Message> messages)
    {
        foreach (Message message in messages)
        {
            var newInstance = new TextObjectUGI(_ChatObject.ChatBehaviour.ContentObject, message);
            this._messagesInstances.Add(newInstance);
        }
    }

    private void ClearChatlogC()
    {
        _messagesInstances.ForEach(message => message.UnityInstance.SelfDestroy());
        _messagesInstances.Clear();
    }

    private void OnTimerTick(object source, EventArgs e)
    {
        _globalTick.SubscribedMembers.Add(this.GetType().Name);
        LoadNewMessages();
    }

    private void LoadNewMessages() // subbed to tick
    {
        if (HasNewMessage) // naffiche pas les 1ers messages car ceci n<est pas called. en 1er dans levent
        {
            var newGameStateMessages = this._gameStateManager.NewMessages;
           // var onlyNewMessages = RemoveExistingMessages(LoadedMessages, newGameStateMessages); // should be empty by now because im not loading every frame
            this.LoadedMessages.AddRange(newGameStateMessages);
            CreateMessageInstances(newGameStateMessages);
        }
    }
}