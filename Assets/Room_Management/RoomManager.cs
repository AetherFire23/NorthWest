using Assets.Room_Management;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using Assets.GameState_Management;
using System.Runtime.CompilerServices;

public class RoomManager : MonoBehaviour
{

    [SerializeField] public Kitchen1Script firstRoomScript;
    [SerializeField] public EntryHallScript secondRoomScript; // tout ecrire la logic dans roomscript, les variables specific cest dans la firstsecond
    [SerializeField] private PlayerScript playerScript;

    public delegate void RoomChangeEventHandler(object source, EventArgs args);
    public event RoomChangeEventHandler OnRoomChanged;

    public RoomScript CurrentRoom;
    private List<RoomScript> RoomScripts = new();

    // DI
    private ClientCalls _clientCalls;
    private GameStateManager _gameStateManager;

    void Start()
    {
        InitRoomsTemplate(); // must be called first

        CurrentRoom = GetCurrentRoomScriptFromGameState();
       // EnableSelectedRoomAndDisableOthers(CurrentRoom);
        playerScript.PlacePlayerCenterRoom(CurrentRoom);
    }

    public void EnableSelectedRoomAndDisableOthers(RoomScript roomScript)
    {
        this.RoomScripts.ForEach(x => x.gameObject.SetActive(false));
        roomScript.gameObject.SetActive(true);
    }

    public RoomScript GetCurrentRoomScriptFromGameState()
    {
        return this.RoomScripts.Single(x => x.RoomName == _gameStateManager.Room.Name);
    }

    public RoomScript GetRoomScriptFromName(string roomName)
    {
        return this.RoomScripts.First(x => x.RoomName == roomName);
    }

    private void InitRoomsTemplate()
    {
        RoomScripts.Add(firstRoomScript);
        RoomScripts.Add(secondRoomScript);
        InitRoomNamesInScripts(); // Doit etre initialise ici parce que Start() run avant le start() des autres scripts...
    }

    private void InitRoomNamesInScripts()
    {
        this.firstRoomScript.RoomName = nameof(LevelTemplate.Kitchen1);
        this.secondRoomScript.RoomName = nameof(LevelTemplate.EntryHall);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Attempting close client.");
        _clientCalls.DestroyClient();
        Debug.Log("Client disposed");
    }

    [Inject]
    public void Construct(GameStateManager gameStateManager, ClientCalls clientCalls)
    {
        _gameStateManager = gameStateManager;
        _clientCalls = clientCalls;
    }

    public void OnRoomChange()
    {
        if (OnRoomChanged is null) return;

        OnRoomChanged(this, EventArgs.Empty);
    }
}
