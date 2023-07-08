using Assets;
using Assets.Dialogs;
using Assets.GameLaunch;
using Assets.HttpStuff;
using Assets.Raycasts;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using UnityEngine;

public class RoomChangerDialogHandler : MonoBehaviour, IStartupBehavior, IRefreshable
{
    [SerializeField] private GameCalls _calls;
    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private RoomChangeManager _roomChangeManager;
    [SerializeField] private LocalPLayerManager _localPLayerManager;
   // [SerializeField] private GameLauncherAndRefresher _gameLauncherAndRefresher;

    private bool _isPrompting => _dialog is not null;
    private YesNoDialog _dialog;
    private GameState _gameState;
    public async UniTask Initialize(GameState gameState)
    {
        _gameState = gameState;
    }

    public async UniTask Refresh(GameState gameState)
    {
        _gameState = gameState;
    }

    public async UniTask Update()
    {
        // could probably add exclusion types for example if the raycast has UI elements or SlotInventory, etc.
        if (_isPrompting) return;
        if (!Input.GetMouseButtonDown(0)) return;

        var door = IIDRaycast.MouseRaycastScriptOrDefault<DoorTargetRoomScript>();
        if (door is null) return;
        //if (door.TargetRoomName.Equals(_gameState.Room.Name))
        //{
        //    Debug.Log("cant acces ssame room ");
        //    return;
        //}

        string test = GetCorrectRoomName(door);

        if (test == string.Empty)
        {
            Debug.Log("Player must be in at least one of the 2 ddoors");
            return;
        }

        // end of guard clauses
        _dialog = await _dialogManager.CreateDialog<YesNoDialog>();
        string message = $"You are about to change room to : {test}. " +
            $"{Environment.NewLine}Unncessary movement will appear suspicious. " +
            $"{Environment.NewLine}This action will be logged as full public.";
        await _dialog.Initialize(message);
        await _dialog.WaitForResolveCoroutine();

        // when it is resolved
        if (!_dialog.DialogResult.Equals(DialogResult.Ok))
        {
            await _dialog.Destroy();
            _dialog = null;
            return;
        }

        var callResult = await _calls.ChangeRoom(PlayerInfo.UID, test);
        await _dialog.Destroy();

        if (!callResult.IsSuccessful)
        {
            Debug.LogError($"Something went wrong in the API when changing door Message: {callResult.Message}");

        }
        else
        {
           // await _gameLauncherAndRefresher.ForceRefreshManagers();
            _roomChangeManager.PlaceLocalPlayerInRoomAndSnapCamera(test);
        }

        _dialog = null;
    }

    //Connections are both-sided. If not the first connection, go to the second one.
    private string GetCorrectRoomName(DoorTargetRoomScript connection)
    {
        var currentRoomName = _gameState.Room.Name;

        bool noDoors = currentRoomName != connection.Connection1 && currentRoomName != connection.Connection2;
        if (noDoors) return string.Empty; // empty means dont show prompt ! 

        string correctDoor = currentRoomName.Equals(connection.Connection1)
            ? connection.Connection2
            : connection.Connection1;
        return correctDoor;
    }
}
