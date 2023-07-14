using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;

namespace Assets.INVENTORY3
{
    public class ItemInventory : PrefabScriptBase, IEntity
    {
        public Guid Id => Item.Id;
        public Item Item { get; set; }
        public SlotInventory Slot { get; set; }
        public bool IsPlayerOwned => Item.OwnerId == PlayerInfo.Id; // holy shit ! 

        public async UniTask Initialize(SlotInventory slotInventory, Item item)
        {
            this.Item = item;
            await slotInventory.InsertItemInSlot(this);
        }

        public async UniTask PlaceItemBackAtSlot()
        {
            await this.Slot.PlaceItemBackAtSlotPosition();
        }
    }
}
