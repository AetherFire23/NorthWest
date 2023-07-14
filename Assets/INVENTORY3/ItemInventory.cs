using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Shared_Resources.Entities;
using Unity.VisualScripting;
using Shared_Resources.Interfaces;

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
