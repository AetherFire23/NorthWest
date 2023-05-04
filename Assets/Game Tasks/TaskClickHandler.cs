//using Assets.GameState_Management;
//using Assets.Input_Management;
//using Assets.InputAwaiter;
//using Assets.Raycasts.NewRaycasts;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Zenject;

//public class TaskClickHandler : MonoBehaviour
//{
//    [Inject] RoomManager _roomManager;
//    private GameStateManager _gameStateManager;
//    private PlayerScript _playerScript;
//    private DialogManager _dialogManager;
//    private InputWaiting _inputWaiter;
//    private ClientCalls _clientCalls;
//    private NewInputManager _newInputManager;
//    private NewRayCaster _newRayCaster;

//    // Start is called before the first frame update


//    // Update is called once per frame
//    async void Update()
//    {
//        if (!_newInputManager.Pressed) return;
//        var rayCast = _newRayCaster.PointerPhysicsRaycast<GameTaskScript>();

//        if (!rayCast.HasFoundHit) return;

//        bool isInRoom = rayCast.HitObject.GetComponent<GameTaskScript>().RoomName == _roomManager.CurrentRoom.RoomName;
//        if (!isInRoom) return;

//        _dialogManager.CreateDialog(DialogType.YesNoDialog, "Do you want to cook ?");

//        await _inputWaiter.WaitForResult();

//        DialogResult result = _dialogManager.CurrentDialog.DialogResult;

//        if (result == DialogResult.Yes || result == DialogResult.Ok)
//        {
//            var t = _clientCalls.CookTask(_gameStateManager.PlayerUID, (rayCast.script as GameTaskScript).StationName);
//            Debug.Log(t);
//        }

//        Debug.Log("Dd");

//    }

//    [Inject]
//    public void Construct(NewRayCaster newRayCaster,
//       NewInputManager newInputManager,
//       ClientCalls clientCalls,
//       InputWaiting inputWaiter,
//       DialogManager dialogManager,
//       PlayerScript playerScript,
//       GameStateManager gameStateManager)
//    {
//        _gameStateManager = gameStateManager;
//        _playerScript = playerScript;
//        _dialogManager = dialogManager;
//        _inputWaiter = inputWaiter;
//        _clientCalls = clientCalls;
//        _newInputManager = newInputManager;
//        _newRayCaster = newRayCaster;
//    }
//}
