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


public class RoomTabBarUGF : UGFWrapper, IInitializable, ITickable
{
    public Guid CurrentRoomGuid
    {
        get { return _chatController.CurrentRoomId; }
        set { _chatController.CurrentRoomId = value; }
    }

    private readonly ChatHandler _chatController;
    private readonly GameStateManager _gameStateManager;
    private List<RoomTabUGI> roomTabs = new();

    public RoomTabBarUGF(RoomTabsContainerObjectScript roomTabsContainerObjScript,
        ChatHandler chatController,
        GameStateManager gameStateManager) : base(roomTabsContainerObjScript)
    {
        _chatController = chatController;
        _gameStateManager = gameStateManager;
    }

    public void Initialize()
    {
        InitializePrivateRoomTabs2();
        this.CurrentRoomGuid = _gameStateManager.LocalPlayerDTO.GameId;
    }

    public void Tick() {}

    public RoomTabUGI BuildRoomTabInstance(Guid roomId) // call this to make a tab instead 
    {
        int roomNumber = this.roomTabs.Count + 1;
        RoomTabUGI newRoomTab = new(this.GameObject, roomId, roomNumber);
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

    private void GiveRoomTabUpdateMethod(RoomTabUGI roomTab)
    {
        roomTab.OnClick.AddListener(() => _chatController.ChangeChatRoom(roomTab.RoomId));
    }
}
