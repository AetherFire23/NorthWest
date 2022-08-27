
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
public class PlusButtonObject : GameObjectWrapper, IInitializable
{
    private readonly PlusButtonObjectScript _plusButtonBehaviour;
    private readonly RoomTabBar _roomTabsContainerObject;
    private readonly ClientCalls _clientCalls;
    private readonly MainPlayer _mainPlayer;

    public PlusButtonObject(PlusButtonObjectScript plusButtonScript, 
        RoomTabBar tabBar,
        ClientCalls clientCalls,
        MainPlayer mainPlayer) : base(plusButtonScript)
    {
        _plusButtonBehaviour = plusButtonScript;
        _roomTabsContainerObject = tabBar;
        _clientCalls = clientCalls;
        _mainPlayer = mainPlayer;
    }

    public void Initialize()
    {
        _plusButtonBehaviour.GetComponent<Button>().AddMethod(AskForNewRoom);
    }

    public void AskForNewRoom()
    {
        Guid newRoomId = Guid.NewGuid();
        _roomTabsContainerObject.BuildRoomTabInstance(newRoomId);
       string responseMessage =  _clientCalls.AddPlayerRoomPair(_mainPlayer.playerModel.Id, newRoomId).AsTask().Result;
    }
}