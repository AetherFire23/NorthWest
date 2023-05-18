using Assets;
using Assets.AssetLoading;
using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.FACTOR3;
using Assets.GameLaunch;
using Assets.Raycasts;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class RoomChangerDialogHandler : MonoBehaviour, IRefreshable
{
    [SerializeField] private Calls _calls;
    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private RoomChangeManager _roomChangeManager;
    // need to roomChange the player.

    private bool _isPrompting => _dialog is not null;
    private YesNoDialog _dialog;


    private GameState _gameState;
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
        if (door.TargetRoomName.Equals(_gameState.Room.Name))
        {
            Debug.Log("cant acces ssame room ");
            return;
        }

        // end of guard clauses
        _dialog = await _dialogManager.CreateDialog<YesNoDialog>();
        string message = $"You are about to change room to : {door.TargetRoomName}. " +
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

        var callResult = await _calls.ChangeRoom(PlayerInfo.UID, door.TargetRoomName);

        if (!callResult.IsSuccessful)
        {
            Debug.LogError($"Something went wrong in the API when changing door Message: {callResult.Message}");
        }
        else
        {
            _roomChangeManager.PlaceLocalPlayerInRoomAndSnapCamera(door.TargetRoomName);
        }

        await _dialog.Destroy();
        _dialog = null;
    }
}
