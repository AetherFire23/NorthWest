using Assets.HttpStuff;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.INVENTORY3
{
    public class InventoryInputClick : MonoBehaviour
    {
        [SerializeField] private InventoryRefreshGuard _refreshGuard;
        [SerializeField] private SlotAndItemsManager _slotAndItemsManager;
        [SerializeField] private InventoryManager3 _inventoryManager;
        [SerializeField] private GameCalls _calls;

        // No more IsHandling stuff ! Just fking refersh 

        public async UniTask Update()
        {
            if (_refreshGuard.MustPreventFromUpdatingInput()) return;

            if (!UIRaycast.TryGetScript(out ItemInventory clickedItem)) return;

            MouseReleaseAction mouseReleaseAction = GetItemReleasedContext(clickedItem);

            await HandleItemSwap(clickedItem, mouseReleaseAction);
        }

        private MouseReleaseAction GetItemReleasedContext(ItemInventory clickedItem)
        {
            MouseReleaseAction action = clickedItem.IsPlayerOwned
                ? MouseReleaseAction.FromPlayerToRoom
                : MouseReleaseAction.FromRoomToPlayer;
            Debug.Log($"MouseResult : {action}");
            return action;
        }

        public async UniTask HandleItemSwap(ItemInventory clickedItem, MouseReleaseAction releaseType)
        {
            switch (releaseType)
            {
                case MouseReleaseAction.FromPlayerToRoom: //ownerShipChange
                    {
                        await _calls.TransferItemOwnerShip(_inventoryManager.CurrentRoomInventoryShownId, clickedItem.OwnerId, clickedItem.Id, PersistenceReducer.GameId);

                        break;
                    }
                case MouseReleaseAction.FromRoomToPlayer: // ownerShipChange
                    {
                        if (!_slotAndItemsManager.HasFreePlayerSlots())
                        {
                            Debug.Log("No more free player slots");
                            return;
                        }

                        await _calls.TransferItemOwnerShip(PersistenceReducer.PlayerId, clickedItem.Item.OwnerId, clickedItem.Id, PersistenceReducer.GameId);
                        break;
                    }
            }
        }
    }
}
