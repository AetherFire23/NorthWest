using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class RoomChangeManager : MonoBehaviour, IStartupBehavior
{
    // the map is important to link the gameState object (roomName) to the Unity gameObject.
    [SerializeField] LocalPLayerManager _localPlayer;
    private Dictionary<string, RoomInfoScript> _roomNameObjectMap = new();

    public async UniTask Initialize(GameState gameState) // attention
    {
        MapRoomNamesAndRoomGameObjects();
    }

    //private void Awake() // dangerous behaviour in Awake, should check if placing this in IInitializable Breaks anything
    //{
    //    // finds all roomInfoScripts in scene so that i can map name  + room 




    //    // TODO:
    //    // warning pour les pieces qui manquent dans le editor
    //}

    public void MapRoomNamesAndRoomGameObjects()
    {
        var rooms = FindObjectsOfType<RoomInfoScript>().ToList();
        foreach (var room in rooms)
        {
            _roomNameObjectMap.Add(room.TargetRoomName, room);
        }
    }

    public void PlaceLocalPlayerInRoomAndSnapCamera(string roomName) // should I load the local player last ?
    {
        var room = _roomNameObjectMap.GetValueOrDefault(roomName);

        if (room is null) throw new System.Exception($"The following room script : {roomName} was not found in the scene");

        var roomPosition = room.transform.position;
        _localPlayer.PlayerPosition = roomPosition;

        Camera.main.transform.position = roomPosition.SetZ(-10);
    }

    public void WarnForMissingRooms(GameState gameState)
    {

    }
}
