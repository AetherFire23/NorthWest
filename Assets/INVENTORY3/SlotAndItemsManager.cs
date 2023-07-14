using Assets.AssetLoading;
using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.INVENTORY3
{
    public class SlotAndItemsManager : MonoBehaviour
    {
        [SerializeField] PrefabLoader _prefabLoader;
        [SerializeField] InventoryStaticGameObjects _inventoryStaticGameObjects;

        private List<SlotInventory> _slots = new List<SlotInventory>();

        public async UniTask<SlotInventory> CreateEmptyPlayerSlot()
        {
            var emptySlot = await _prefabLoader.CreateInstanceOfAsync<SlotInventory>(_inventoryStaticGameObjects.PlayerInventoryPanel.gameObject);
            await emptySlot.Initialize(false);
            _slots.Add(emptySlot);
            return emptySlot;
        }

        public async UniTask<SlotInventory> CreateNewRoomSlotAndCreateNewItem(Item item)
        {
            var slot = await _prefabLoader.CreateInstanceOfAsync<SlotInventory>(_inventoryStaticGameObjects.RoomInventoryScrollView.gameObject);
            await slot.Initialize(true);

            var itemInventory = await _prefabLoader.CreateInstanceOfAsync<ItemInventory>(slot.gameObject);
            await itemInventory.Initialize(slot, item); // is inserted in there
            _slots.Add(slot);
            return slot;
        }

        public async UniTask<SlotInventory> CreateRoomInventorySlotAndInsertItem(ItemInventory item) // should never be empty though...
        {
            var slot = await _prefabLoader.CreateInstanceOfAsync<SlotInventory>(_inventoryStaticGameObjects.RoomInventoryScrollView.gameObject);
            await slot.Initialize(true);
            await slot.InsertItemInSlot(item);
            _slots.Add(slot);
            return slot;
        }

        public List<ItemInventory> GetPlayerItems()
        {
            var items = _slots
                .Where(x => x.HasItem) // avoids null ref access
                .Where(x => x.Item.IsPlayerOwned)
                .Select(x => x.Item)
                .ToList();
            return items;
        }

        public List<ItemInventory> GetRoomItems()
        {
            var items = _slots
                .Where(x => x.HasItem) // avoids null ref access
                .Where(x => !x.Item.IsPlayerOwned)
                .Select(x => x.Item)
                .ToList();
            return items;
        }

        public List<SlotInventory> GetPlayerSlots()
        {
            var slots = _slots.Where(x => !x.IsRoomSlot).ToList();
            return slots;
        }

        public int GetFreePlayerSlotCount()
        {
            int slotCount = _slots.Where(x => !x.IsRoomSlot && !x.HasItem).Count();
            return slotCount;
        }
        public List<SlotInventory> GetFreePlayerSlots()
        {
            List<SlotInventory> freeSlots = _slots.Where(x => !x.IsRoomSlot && !x.HasItem).ToList();
            return freeSlots;
        }

        public SlotInventory FindFreePlayerSlotOrDefault()
        {
            var allFreeSlots = GetFreePlayerSlots();

            var freeSlot = allFreeSlots.Any() ? allFreeSlots[0] : null;
            return freeSlot;
        }

        public bool HasFreePlayerSlots()
        {
            var freePlayerSlot = FindFreePlayerSlotOrDefault();
            bool isFree = freePlayerSlot is not null;
            return isFree;
        }

        public void RemoveSlot(SlotInventory slot)
        {
            _slots.Remove(slot);
        }
    }
}
