using Assets.Inventory.Slot;
using Assets.Utils;
using Shared_Resources;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Inventory.Player_Item
{
    public class ItemUGI : InstanceWrapper<ItemScript>, IEntity
    {
        public SlotUGI CurrentSlot
        {
            get
            {
                var slotScript = this.UnityInstance.GetComponentInParent<SlotScript>();
                if (slotScript is null) throw new Exception("An item can not have no slot.");

                return slotScript.SelfWrapper;
            }
        }

        public Guid Id => Item.Id;
        public const string resourceName = "ItemInstance";
        public Item Item;

        public ItemUGI(GameObject parent, Item item) : base(resourceName, parent)
        {
            Item = item;
            SetTextureFromEnum(item.ItemType);
            this.AccessScript.selfWrapper = this; // set reference to this 
        }

        public void SetTextureFromEnum(ItemType itemid)
        {
            switch(itemid)
            {
                case ItemType.Wrench:
                    {
                        Sprite wrenchSprite = Resources.Load<Sprite>("WrenchSprite");

                        if (wrenchSprite == null)
                        {
                            throw new NotImplementedException($"The sprite you tried to load {wrenchSprite.name} was null. Check resources names.");
                        }
                        this.UnityInstance.GetComponent<Image>().sprite = wrenchSprite;
                        break;
                    }
            }
        }
    }
}