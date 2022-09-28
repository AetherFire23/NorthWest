using Assets.InputAwaiter;
using Assets.Raycasts;
using Assets.RoomTransitioning.Room_Instances;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.RoomTransitioning
{
    public class RoomSwitchHandler : ITickable
    {
        private readonly DialogManager _dialogManager;
        private readonly PlayerInputState _playerInputState;
        private readonly PlayerScript _playerScript;
        private readonly RoomHandler _roomHandler;
        private readonly MainPlayer _mainPlayer;
        private readonly InputWaiting _inputWaiting;
        private readonly ClientCalls _clientCalls;

        public RoomSwitchHandler(DialogManager dialogManager,
            PlayerInputState playerInputState,
            PlayerScript playerScript,
            RoomHandler roomHandler,
            MainPlayer mainPlayer,
            InputWaiting inputWaiting,
            ClientCalls clientCalls)
        {
            _dialogManager = dialogManager;
            _playerInputState = playerInputState;
            _playerScript = playerScript;
            _roomHandler = roomHandler;
            _mainPlayer = mainPlayer;
            _inputWaiting = inputWaiting;
            _clientCalls = clientCalls;
        }

        public async void Tick() // This shows the yesnodialog
        {
            if (!_dialogManager.CanInvokeNewInstance()) return;
            if (!Input.GetMouseButton(0)) return;

            IIDRaycastResult rayResult = IIDRaycast.MouseRaycast(hit => hit.CompareTag("Door"));
            if (!rayResult.HasFoundHit) return;

            _dialogManager.CreateDialog(DialogType.YesNoDialog, "Do you want to spend 1 action point ? ");

            await _inputWaiting.WaitForResult();

            if (_dialogManager.DialogResult == DialogResult.Ok) // changed 
            {
                RoomType targetRoomType = rayResult.HitObject.GetComponent<DoorBehaviour>().RoomAccessType;
                SwitchRooms(targetRoomType); // a date je me sers pas des adjacents comme un retard haha , mais je pourrai copy-paste les adjacents 
                Debug.Log("Player would spend an action point if it were an api call");
            }
            _dialogManager.CurrentDialog = null; // must null AFTER using that unless will conflit with CanInvokeNewInstance
        }

        public void SwitchRooms(RoomType targetRoomType)
        {
            RoomObject targetRoom = _roomHandler.Rooms.FirstOrDefault(room => room.roomType == targetRoomType);
            RoomObject currentRoom = _roomHandler.Rooms.FirstOrDefault(room => room.roomType == _mainPlayer.CurrentRoomType);

            Vector3 targetPosition = targetRoom.CenterPosition;
            _playerScript.transform.position = targetPosition;
            _playerInputState.TargetPosition = targetPosition; // set le targetposition pour ne pas que y veulent revenir automatic ou est la souris 

            targetRoom.AccessScript.gameObject.SetActive(true);
            currentRoom.AccessScript.gameObject.SetActive(false);
            _mainPlayer.CurrentRoomType = targetRoomType;
        }
    }
}
