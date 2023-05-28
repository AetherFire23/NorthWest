using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.INVENTORY3
{
    public class InventoryRefreshGuard : MonoBehaviour
    {
        [SerializeField] private InventoryInput _inventoryInput;

        public bool MustWaitForNextGameState { get; set; } = false;
        public bool IsInitialized { get; set; } = false;
        public bool IsRefreshing { get; set; } = false;
        public bool IsSwitchingRoomInventory { get; set; } = false;
        public bool IsTracking => _inventoryInput.IsTracking;

        private bool _hasItemOnClick => UIRaycast.ScriptOrDefault<ItemInventory>() is not null;
        private bool _isMouseClicked => Input.GetMouseButtonDown(0);
        public bool MustPreventRefresh()
        {
            if (!IsInitialized || IsRefreshing || IsSwitchingRoomInventory || IsTracking || MustWaitForNextGameState)
            {
                if (IsSwitchingRoomInventory)
                    Debug.LogError("Refresh canceled because it is switching inventory");
                else if (IsTracking)
                    Debug.LogError("Refresh canceled due to input handling");
                else if (MustWaitForNextGameState)
                {
                    MustWaitForNextGameState = false; // Allow refresh next refresh
                    Debug.Log("Refresh was canceled because an operation needs the newest version of the GameState");
                }
                return true;
            }
            return false;
        }

        public bool MustPreventChangingRoom()
        {
            if (IsSwitchingRoomInventory)
            {
                return true;
            }

            return false;
        }

        public bool MustPreventFromUpdatingInput()
        {
            if (!IsInitialized || IsRefreshing || IsTracking || !_isMouseClicked || !_hasItemOnClick)
            {
                return true;
            }

            return false;
        }

        public async UniTask WaitUntilRefreshEndsCoroutine()
        {
            while (this.IsRefreshing)
            {
                Debug.Log("Waiting for refresh before handling manipulation");
                await UniTask.Yield();
            }
        }

        public async UniTask WaitUntilInputEndsCoroutine()
        {
            while (this.IsTracking)
            {
                Debug.Log("Waiting Until Refresh Ends");
                await UniTask.Yield();
            }
        }
    }
}
