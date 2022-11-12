using Assets.GameState_Management;
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
    private ClientCalls _clientCalls;
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
            var t = _newRayCaster.PointerPhysicsRaycast<DoorScript>();


            if (!t.HasFoundHit) return;
            if (_dialogManager.IsWaitingForInput()) return;
            string targetRoomName = t.HitObject.GetComponent<DoorScript>().targetRoom;

            _dialogManager.CreateDialog(DialogType.YesNoDialog, $"Are you sure you wanna change rooms to : {targetRoomName}");

            await _inputWaiter.WaitForResult();

            string webAPiResult = _clientCalls.ChangeRoom(_gameStateManager.PlayerUID, targetRoomName);

            // si reussite, tp le player dans la room

            bool mustChangeRoom = webAPiResult == "Tu as change de room!"; // faudra faire une TaskResponse model

            if (mustChangeRoom)
            {
                RoomScript nextRoomScript = _roomManager.GetRoomScriptFromName(targetRoomName);
                _roomManager.EnableSelectedRoomAndDeactivateOthers(nextRoomScript);
               _playerScript.PlacePlayerCenterRoom(nextRoomScript);
                _roomManager.OnRoomChange();
            }
            Debug.Log($"Web APi result : {webAPiResult}");
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
        _clientCalls = clientCalls;
        _newInputManager = newInputManager;
        _newRayCaster = newRayCaster;
    }
}
