using Assets;
using Assets.GameState_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


public class RoomTabBar : GameObjectWrapper, IInitializable, ITickable
{
    public Guid CurrentRoomGuid
    {
        get { return _chatController.CurrentRoomId; }
        set { _chatController.CurrentRoomId = value; }
    }

    private readonly MainPlayer _mainPlayer;
    private readonly ChatHandler _chatController;
    private readonly GameStateManager _gameStateManager;
    private List<RoomTabInstance> roomTabs = new();

    public RoomTabBar(RoomTabsContainerObjectScript roomTabsContainerObjScript,
        MainPlayer mainPlayer,
        ChatHandler chatController,
        GameStateManager gameStateManager) : base(roomTabsContainerObjScript)
    {
        _mainPlayer = mainPlayer;
        _chatController = chatController;
        _gameStateManager = gameStateManager;
    }

    public void Initialize()
    {
        InitializePrivateRoomTabs2();
        this.CurrentRoomGuid = _mainPlayer.playerModel.GameId;
    }

    public void Tick() {}

    public RoomTabInstance BuildRoomTabInstance(Guid roomId) // call this to make a tab instead 
    {
        int roomNumber = this.roomTabs.Count + 1;
        RoomTabInstance newRoomTab = new(this.GameObject, roomId, roomNumber);
        GiveRoomTabUpdateMethod(newRoomTab);
        this.roomTabs.Add(newRoomTab);
        return newRoomTab;
    }

    private void InitializePrivateRoomTabs2()
    {
        List<Guid> roomsWithoutDuplicates = _gameStateManager.ParticipantsInPlayerRooms.Select(p => p.RoomId).Distinct().ToList();
        InstantiateTabsForAllRooms(roomsWithoutDuplicates);
    }

    private void InstantiateTabsForAllRooms(List<Guid> privateRoomsGuids) // faut initialiser le bouton a lexterieur pour pouvoir updater le current guid ! 
    {
        for (int i = 0; i < privateRoomsGuids.Count; i++)
        {
            var currentGuid = privateRoomsGuids[i];
            BuildRoomTabInstance(currentGuid);
        }
    }

    private void GiveRoomTabUpdateMethod(RoomTabInstance roomTab)
    {
        roomTab.OnClick.AddListener(() => _chatController.ChangeChatRoom(roomTab.RoomId));
    }
}
