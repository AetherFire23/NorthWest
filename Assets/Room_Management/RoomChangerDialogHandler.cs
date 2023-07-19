using Assets;
using Assets.Dialogs;
using Assets.FullTasksPanel;
using Assets.GameLaunch.BaseLauncherScratch;
using Assets.HttpStuff;
using Assets.Raycasts;
using Assets.Scratch;
using Assets.SSE;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using UnityEngine;

public class RoomChangerDialogHandler : StateHolderBase<GameState>
{
    [SerializeField] private GameCalls _calls;
    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private RoomChangeManager _roomChangeManager;
    [SerializeField] private LocalPLayerManager _localPLayerManager;
    [SerializeField] private GameDataStore _gameDataStore;
    [SerializeField] private GameManagersContainer _gameManagers;
    // [SerializeField] private GameLauncherAndRefresher _gameLauncherAndRefresher;

    private bool _isPrompting => _dialog is not null;
    private YesNoDialog _dialog;
    private GameState _gameState;
    public override async UniTask Initialize(GameState gameState)
    {
        _gameState = gameState;
        base.InitializationValues.IsInitialized = true;
    }

    public override async UniTask Refresh(GameState gameState)
    {
        _gameState = gameState;
    }

    public async UniTask Update()
    {
        // could probably add exclusion types for example if the raycast has UI elements or SlotInventory, etc.
        if (!InitializationValues.IsInitialized) return;
        if (_isPrompting) return;
        if (!Input.GetMouseButtonDown(0)) return;

        //var door = IIDRaycast.MouseRaycastScriptOrDefault<DoorTargetRoomScript>();
        //if (door is null) return;

        if (!IIDRaycast.TryGetScript<DoorTargetRoomScript>(out var door)) return;

        string roomName = GetCorrectRoomName(door);

        if (roomName == string.Empty)
        {
            Debug.Log("Player must be in at least one of the 2 ddoors");
            return;
        }

        // end of guard clauses
        _dialog = await _dialogManager.CreateDialog<YesNoDialog>();
        string message = $"You are about to change room to : {roomName}. " +
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

        var callResult = await _calls.ChangeRoom(PersistenceReducer.PlayerId, roomName);
        // techniquement ca devrait tt le temps etre des events mesemble.
        // Genre ca resoudrait tous les conflits de prediction et tout 
        await _dialog.Destroy();

        if (!callResult.IsSuccessful)
        {
            Debug.LogError($"Something went wrong in the API when changing door Message: {callResult.Message}");

        }
        else
        {
            _gameDataStore.UpdateLocalPlayerRoomId(roomName); // for prediction 
            _roomChangeManager.PlaceLocalPlayerInRoomAndSnapCamera(roomName);
            await _gameManagers.RefreshSpecificManager<FullTasksManager>();
        }

        _dialog = null;
    }

    //Connections are both-sided. If not the first connection, go to the second one.
    private string GetCorrectRoomName(DoorTargetRoomScript connection)
    {
        var currentRoomName = _gameState.LocalPlayerRoom.Name;

        bool noDoors = currentRoomName != connection.Connection1 && currentRoomName != connection.Connection2;
        if (noDoors) return string.Empty; // empty means dont show prompt ! 

        string correctDoor = currentRoomName.Equals(connection.Connection1)
            ? connection.Connection2
            : connection.Connection1;
        return correctDoor;
    }
}
