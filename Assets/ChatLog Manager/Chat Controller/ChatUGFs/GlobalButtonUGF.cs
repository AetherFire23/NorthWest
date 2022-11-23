//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Zenject;
//using UnityEngine;
//using Assets;
//using Cysharp.Threading.Tasks;
//using Assets.GameState_Management;

//public class GlobalButtonUGF : UGFWrapper, IInitializable
//{
//    private GlobalButtonObjectScript _globalButtonObjectScript;
//    private readonly RoomTabBarUGF _roomTabsContainerObject;
//    private readonly ClientCalls _clientCalls;
//    private readonly ChatHandler _chatController;
//    private readonly GameStateManager _gameStateManager;
//    public GlobalButtonUGF(GlobalButtonObjectScript script,
//        RoomTabBarUGF roomTabs,
//        ClientCalls clientCalls,
//        ChatHandler chatController,
//        GameStateManager gameStateManager) : base(script) // injected ! 
//    {
//        this._globalButtonObjectScript = script;
//        _roomTabsContainerObject = roomTabs;
//        _clientCalls = clientCalls;
//        _chatController = chatController;
//        _gameStateManager = gameStateManager;
//    }

//    public void Initialize()
//    {
//        InitGlobalChatButton();
//    }

//    public void InitGlobalChatButton()
//    {
//        _globalButtonObjectScript.button.onClick.AddListener(() => ReturnToGlobalChat());

//    }

//    public void ReturnToGlobalChat()
//    {
//        _chatController.ChangeChatRoom(_gameStateManager.LocalPlayerDTO.GameId);
//    }
//}

