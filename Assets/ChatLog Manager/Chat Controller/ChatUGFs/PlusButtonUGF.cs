
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using Zenject;
using Assets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Assets.GameState_Management;

public class PlusButtonUGF : UGFWrapper, IInitializable
{
    private readonly PlusButtonObjectScript _plusButtonBehaviour;
    private readonly RoomTabBarUGF _roomTabsContainerObject;
    private readonly ClientCalls _clientCalls;
    private readonly GameStateManager _gameStateManager;

    public PlusButtonUGF(PlusButtonObjectScript plusButtonScript, 
        RoomTabBarUGF tabBar,
        ClientCalls clientCalls,
        GameStateManager gameStateManager) : base(plusButtonScript)
    {
        _gameStateManager = gameStateManager;
        _plusButtonBehaviour = plusButtonScript;
        _roomTabsContainerObject = tabBar;
        _clientCalls = clientCalls;
    }

    public void Initialize()
    {
        _plusButtonBehaviour.GetComponent<Button>().AddMethod(AskForNewRoom);
    }

    public void AskForNewRoom()
    {
        Guid newRoomId = Guid.NewGuid();
        _roomTabsContainerObject.BuildRoomTabInstance(newRoomId);
       string responseMessage =  _clientCalls.AddPlayerRoomPair(_gameStateManager.PlayerUID, newRoomId).AsTask().Result;
    }
}