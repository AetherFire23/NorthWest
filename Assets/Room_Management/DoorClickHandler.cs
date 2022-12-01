using Assets.GameState_Management;
using Assets.HttpClient.Shared_API_Models;
using Assets.Input_Management;
using Assets.InputAwaiter;
using Assets.Raycasts.NewRaycasts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DoorClickHandler : MonoBehaviour
{
    [SerializeField] RoomManager _roomManager;

    private NewInputManager _newInputManager;
    private NewRayCaster _newRayCaster;
    private ClientCalls _client;
    private PlayerScript _playerScript;
    private DialogManager _dialogManager;
    private InputWaiting _inputWaiter;
    GameStateManager _gameStateManager;
    public void Start()
    {
    }

    async void Update()
    {
        if (_newInputManager.Pressed)
        {
            //  var t = _newRayCaster.PointerPhysicsRaycast(x => x.transform.gameObject.tag == "Door");
            var doors = _newRayCaster.PointerPhysicsRaycast<DoorScript>();

            if (!doors.HasFoundHit) return;
            if (_dialogManager.IsWaitingForInput()) return;

            string targetRoomName = doors.HitObject.GetComponent<DoorScript>().targetRoom;
            _dialogManager.CreateDialog(DialogType.YesNoDialog, $"Are you sure you wanna change rooms to : {targetRoomName}");

            await _inputWaiter.WaitForResult();

            if (_dialogManager.DialogResult == DialogResult.Cancel)
            {
                _dialogManager.CurrentDialog = null;
                return;
            }

            ClientCallResult callResult = _client.ChangeRoom(_gameStateManager.PlayerUID, targetRoomName);

            // si reussite, tp le player dans la room

            //
            if (callResult.IsSuccessful)
            {
                RoomScript nextRoomScript = _roomManager.GetRoomScriptFromName(targetRoomName);
                _roomManager.EnableSelectedRoomAndDisableOthers(nextRoomScript);
                _playerScript.PlacePlayerCenterRoom(nextRoomScript);
                _roomManager.OnRoomChange();
            }

            Debug.Log($"Web APi result : {callResult.Message}");
            _dialogManager.CurrentDialog = null;

        }
    }

    [Inject]
    public void Construct(NewRayCaster newRayCaster,
        NewInputManager newInputManager,
        ClientCalls clientCalls,
        GameStateManager gameStateManager,
        InputWaiting inputWaiter,
        DialogManager dialogManager,
        PlayerScript playerScript)
    {
        _playerScript = playerScript;
        _dialogManager = dialogManager;
        _inputWaiter = inputWaiter;
        _gameStateManager = gameStateManager;
        _client = clientCalls;
        _newInputManager = newInputManager;
        _newRayCaster = newRayCaster;
    }
}
