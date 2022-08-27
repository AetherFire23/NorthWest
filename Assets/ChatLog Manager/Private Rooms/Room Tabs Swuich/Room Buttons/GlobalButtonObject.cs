using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using Assets;
using Cysharp.Threading.Tasks;

public class GlobalButtonObject : GameObjectWrapper, IInitializable
{
    private GlobalButtonObjectScript _globalButtonObjectScript;
    private readonly RoomTabBar _roomTabsContainerObject;
    private readonly MainPlayer _mainPlayer;
    private readonly ClientCalls _clientCalls;
    private readonly ChatHandler _chatController;
    public GlobalButtonObject(GlobalButtonObjectScript script,
        RoomTabBar roomTabs,
        MainPlayer mainPlayer,
        ClientCalls clientCalls,
        ChatHandler chatController) : base(script) // injected ! 
    {
        this._globalButtonObjectScript = script;
        _mainPlayer = mainPlayer;
        _roomTabsContainerObject = roomTabs;
        _clientCalls = clientCalls;
        _chatController = chatController;
    }

    public void Initialize()
    {
        // _globalButtonObjectScript.button.onClick.AddListener(delegate { ReturnToGlobalChat(); });
         _globalButtonObjectScript.button.onClick.AddListener(() => ReturnToGlobalChat2());

    }

    public void ReturnToGlobalChat()
    {
        Guid gameId = _mainPlayer.playerModel.GameId;
        _roomTabsContainerObject.CurrentRoomGuid = gameId;
        var yeah = _clientCalls.UpdateCurrentRoomId(_mainPlayer.playerModel.Id, gameId).AsTask().Result;
      //  _chatController.ResetMessages();
    }
    public void ReturnToGlobalChat2()
    {
        var gameId = _mainPlayer.GameId;
        _chatController.ChangeChatRoom(gameId);
    }
}

