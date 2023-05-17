using Assets.GameLaunch;
using Assets.GameState_Management;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandmassPositionManager : MonoBehaviour, IStartupBehavior
{
    private Dictionary<string, RoomInfoScript> _landmassRoomObjectNames = new();

    // TODO:
    // faire les procedural doors 

    public async UniTask Initialize(GameState gameState)
    {
        var landmassRoomNames = gameState.GetLandmassRooms().Select(x => x.Name).ToList();

        var landmassRoomInfoScripts = FindObjectsOfType<RoomInfoScript>().Where(x => landmassRoomNames.Contains(x.name));

        foreach (var room in landmassRoomInfoScripts)
        {
            _landmassRoomObjectNames.Add(room.TargetRoomName, room);
        }
       await PlaceLandmassRoomsAtCorrectPositionInSpace(gameState);
    }

    public async UniTask PlaceLandmassRoomsAtCorrectPositionInSpace(GameState gameState)
    {
        var landmassRooms = gameState.GetLandmassRooms();
        foreach (var landmassRoom in landmassRooms)
        {
            RoomInfoScript gameObjectRoom = _landmassRoomObjectNames.GetValueOrDefault(landmassRoom.Name);

            if(gameObjectRoom is null)
            {
                Debug.LogError($"The following landmassRoom : {landmassRoom.Name} could not be found in the scene.");
                continue;
            }
            float offsetForFun = 2;
            gameObjectRoom.transform.position = new Vector2(landmassRoom.X *offsetForFun, landmassRoom.Y * offsetForFun);
        }
    }
}
