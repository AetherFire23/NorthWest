//using Assets.Room_Management;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//using Zenject;
//using Assets.GameState_Management;
//using System.Runtime.CompilerServices;

//public class RoomManager : MonoBehaviour
//{
//    // Editor variables 
//    [SerializeField] private PlayerScript playerScript; // pour bouger le perso principal quand il change de room
//    [SerializeField] List<RoomScript> RoomScripts; // Les roomScript sont faits a la main dans le editor

//    // Local variables 
//    public RoomScript CurrentRoom;

//    // Event 
//    public delegate void RoomChangeEventHandler(object source, EventArgs args);
//    public event RoomChangeEventHandler OnRoomChanged;

//    // DI
//    private ClientCalls _clientCalls;
//    private GameStateManager _gameStateManager;

//    void Start()
//    {
//        CurrentRoom = GetCurrentRoomScriptFromGameState();
//        playerScript.PlacePlayerCenterRoom(CurrentRoom);
//       // InitDbRooms();
//    }

//    public void InitDbRooms()
//    {
//        foreach (var room in this.RoomScripts)
//        {
//            var dbRoom = this._gameStateManager.Rooms.First(x => x.Name == room.name);
//            room.RoomDTO = dbRoom;
//        }
//    }

//    public void EnableSelectedRoomAndDisableOthers(RoomScript roomScript) // porlly plus jamais utilisee vu que c rendu un gros board game 
//    {
//        this.RoomScripts.ForEach(x => x.gameObject.SetActive(false));
//        roomScript.gameObject.SetActive(true);
//    }

//    public RoomScript GetCurrentRoomScriptFromGameState()
//    {
//        return this.RoomScripts.Single(x => x.RoomName == _gameStateManager.Room.Name);
//    }

//    public RoomScript GetRoomScriptFromName(string roomName)
//    {
//        return this.RoomScripts.First(x => x.RoomName == roomName);
//    }

//    private void OnApplicationQuit()
//    {
//        Debug.Log("Attempting close client.");
//        _clientCalls.DestroyClient();
//        Debug.Log("Client disposed");
//    }

//    public void CheckExpeditionDoorVisibility()
//    {
//        throw new NotImplementedException();
//    }

//    public void CheckExpeditionRoomVisibility()
//    {
//        throw new NotImplementedException();
//    }

//    public void CheckExpeditionStationVisibility()
//    {
//        throw new NotImplementedException();
//    }

//    public void CheckExpeditionEnded()
//    {
//        throw new NotImplementedException();
//    }

//    private void OnTimerTick(object source, EventArgs e)
//    {
//        CheckExpeditionDoorVisibility();
//        CheckExpeditionRoomVisibility();
//        CheckExpeditionStationVisibility(); // Should include taks I guess 
//    }

//    [Inject]
//    public void Construct(GameStateManager gameStateManager, ClientCalls clientCalls)
//    {
//        _gameStateManager = gameStateManager;
//        _clientCalls = clientCalls;
//    }

//    public void OnRoomChange()
//    {
//        if (OnRoomChanged is null) return;

//        OnRoomChanged(this, EventArgs.Empty);
//    }
//}
