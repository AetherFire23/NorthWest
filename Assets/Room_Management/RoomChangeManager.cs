using Assets.GameLaunch;
using Assets.GameLaunch.BaseLauncherScratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomChangeManager : StateHolderBase<GameState>
{
    // the map is important to link the gameState object (roomName) to the Unity gameObject.
    [SerializeField] LocalPLayerManager _localPlayer;
    private Dictionary<string, RoomInfoScript> _roomNameObjectMap = new();

    public override async UniTask Initialize(GameState gameState) // attention
    {
        MapRoomNamesAndRoomGameObjects();
        PlaceLocalPlayerInRoomAndSnapCamera(gameState.Room.Name);
    }

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
