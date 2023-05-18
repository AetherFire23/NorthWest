using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

// no http calls, just changing the player position
public class RoomChangeManager : MonoBehaviour
{
    // the map is important to link the gameState object (roomName) to the Unity gameObject.
    private Dictionary<string, RoomInfoScript> _roomNameObjectMap = new();
    [SerializeField] LocalPLayerManager _localPlayer;
    private void Awake() // dangerous behaviour in Awake, should check if placing this in IInitializable Breaks anything
    {
        // finds all roomInfoScripts in scene so that i can map name  + room 
        var rooms = FindObjectsOfType<RoomInfoScript>().ToList();
        foreach (var room in rooms)
        {
            _roomNameObjectMap.Add(room.TargetRoomName, room);
        }

        

        // TODO:
        // warning pour les pieces qui manquent dans le editor
    }

    public void PlaceLocalPlayerInRoomAndSnapCamera(string roomName) // should I load the local player last ?
    {
        var room = _roomNameObjectMap[roomName];
        var roomPosition = room.transform.position;
        _localPlayer.PlayerPosition = roomPosition;

        Camera.main.transform.position = roomPosition.SetZ(-10);
    }
}
