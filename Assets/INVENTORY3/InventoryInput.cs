using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.INVENTORY3
{
    public class InventoryInput : MonoBehaviour
    {
        [SerializeField] private InventoryStaticGameObjects _inventoryObjects;
        [SerializeField] private SlotAndItemsManager _slotAndItemsManager;
        [SerializeField] private InventoryManager3 _inventoryManager;
        [SerializeField] private InventoryRefreshGuard _refreshGuard;
        [SerializeField] private GameCalls _calls;
        [SerializeField] private GameLauncherAndRefresher _gameLauncherAndRefresher;


        private ItemInventory _trackedItem = null;
        public bool IsTracking => _trackedItem != null;

        public async UniTask Update()
        {
            if (_refreshGuard.MustPreventFromUpdatingInput()) return;

            _trackedItem = UIRaycast.ScriptOrDefault<ItemInventory>();

            // All refresh operations are cancelled when _trackItem is not null so not to cause flickering.
            await HandleItemTrackingUntilMouseReleaseCoroutine();
            MouseReleaseAction mouseReleaseAction = await GetItemReleasedContext();
            if (mouseReleaseAction.Equals(MouseReleaseAction.Invalid))
            {
                await _trackedItem.PlaceItemBackAtSlot();
                _trackedItem = null;
                Debug.Log(" was not a valid release");
                return;
            }

            await HandleItemSwap(mouseReleaseAction);

            _trackedItem = null; // in all cases, when the button is lifted up, should stop tracking everything.
            await _gameLauncherAndRefresher.ForceRefreshManagers(); // Tracked item set to false is necessary for refresh 
        }

        public async UniTask HandleItemSwap(MouseReleaseAction releaseType)
        {
            switch (releaseType)
            {
                case MouseReleaseAction.FromPlayerToPlayer:
                    {
                        var targetSlot = UIRaycast.ScriptOrDefault<SlotInventory>();
                        await targetSlot.InsertItemInSlot(_trackedItem);
                        break;
                    }
                case MouseReleaseAction.FromPlayerToRoom: //ownerShipChange
                    {
                        _refreshGuard.MustWaitForNextGameState = true; // gamestate might be out of date right when it comes in thus creating item flickering
                        var slot = await _slotAndItemsManager.CreateRoomInventorySlotAndInsertItem(_trackedItem);
                        // For prediction
                        _trackedItem.Item.OwnerId = _inventoryManager.CurrentInventoryShownRoomId;
                        await _calls.TransferItemOwnerShip(_inventoryManager.CurrentInventoryShownRoomId, _trackedItem.Id);
                        break;
                    }
                case MouseReleaseAction.FromRoomToPlayer: // ownerShipChange
                    {
                        _refreshGuard.MustWaitForNextGameState = true;
                        var oldInventorySlot = _trackedItem.Slot;
                        var playerSlot = UIRaycast.ScriptOrDefault<SlotInventory>();
                        await playerSlot.InsertItemInSlot(_trackedItem);
                        await oldInventorySlot.DestroySlot();
                        _trackedItem.Item.OwnerId = PlayerInfo.UID;
                        await _calls.TransferItemOwnerShip(PlayerInfo.UID, _trackedItem.Id);
                        break;
                    }
                case MouseReleaseAction.FromRoomToRoom:
                    {
                        await _trackedItem.PlaceItemBackAtSlot();
                        break;
                    }
            }
        }

        private async UniTask HandleItemTrackingUntilMouseReleaseCoroutine()
        {
            // changing the tracked item parent to prevent the mask from hiding the gameObject.
            _trackedItem.transform.parent = _inventoryObjects.RoomInventoryCanvas.transform;
            while (Input.GetMouseButton(0) && !_refreshGuard.IsRefreshing)
            {
                _trackedItem.Position = Input.mousePosition;
                await UniTask.Yield();
            }
            _trackedItem.transform.SetParent(_trackedItem.Slot.gameObject.transform);
        }

        // what Happened when the mouse button was lifted ?
        private async UniTask<MouseReleaseAction> GetItemReleasedContext()
        {
            if (IsInvalidRelease()) return MouseReleaseAction.Invalid;


            bool isOverRoomInventory = UIRaycast.TagExists("RoomInventory");
            if (isOverRoomInventory)
            {
                if (_trackedItem.IsPlayerOwned) return MouseReleaseAction.FromPlayerToRoom;
                else return MouseReleaseAction.FromRoomToRoom;
            }

            else // player was already checked so it must work
            {
                if (_trackedItem.IsPlayerOwned) return MouseReleaseAction.FromPlayerToPlayer;
                else return MouseReleaseAction.FromRoomToPlayer;
            }
        }

        private bool IsInvalidRelease()
        {
            // check if on another item
            // should I make a class ????
            var otherItems = UIRaycast.MouseRaycastResult(x => x.gameObject.GetComponent<ItemInventory>() is not null)
                .raycastResults.Select(x => x.gameObject.GetComponent<ItemInventory>())
                .Where(x => x != _trackedItem);

            var isOverAnotherItem = otherItems
                .Any();

            if (isOverAnotherItem) return true;

            // check if the slot that its been released on is the same item thats being tracked
            var slotBehind = UIRaycast.ScriptOrDefault<SlotInventory>();
            if (slotBehind == _trackedItem.Slot) return true;

            // Check if the item is being dropped on a SLOT (that contains an item
            // && operator is important because if I just check for .HasItem when dragging on inventory,
            // you get a null ref exception
            if (slotBehind != null && slotBehind.HasItem) return true; // 

            // check if release is either over room or player, if none, it is invalid (anywhere else on the screen)
            bool isOverRoomInventory = UIRaycast.TagExists("RoomInventory");
            bool isOverPlayerSlot = UIRaycast.ScriptOrDefault<SlotInventory>() is not null;
            bool eitherOnPlayerOrInventory = isOverRoomInventory || isOverPlayerSlot;
            if (!eitherOnPlayerOrInventory) return true;

            return false;
        }
    }
}
