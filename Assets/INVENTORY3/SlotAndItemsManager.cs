using Assets.AssetLoading;
using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void RemoveSlot(SlotInventory slot)
        {
            _slots.Remove(slot);
        }
    }
}
