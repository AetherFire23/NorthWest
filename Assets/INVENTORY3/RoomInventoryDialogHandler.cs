using Assets.Dialogs;
using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.Raycasts;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.INVENTORY3
{
    public class RoomInventoryDialogHandler : MonoBehaviour
    {
        [SerializeField] private InventoryManager3 _inventoryManager;
        [SerializeField] private DialogManager _dialogManager;

        private bool _isChangingRoom = false;
        async UniTask Update()
        {
            if (_isChangingRoom) return;
            if (!Input.GetMouseButtonDown(0)) return;

            var roomChest = IIDRaycast.MouseRaycastScriptOrDefault<RoomChest>();
            if (roomChest is null) return;
            //if (roomChest.RoomChestName.Equals(_inventoryManager.CurrentShownRoom.Name)) return;

            _isChangingRoom = true;

            await _inventoryManager.SetRoomInventory(roomChest.RoomChestName);

            _isChangingRoom = false;
        }
    }
}
