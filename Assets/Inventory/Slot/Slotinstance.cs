using Assets.Inventory.Models;
using Assets.Inventory.Player_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Inventory.Slot
{
    public class SlotInstance : InstanceWrapper<SlotScript>
    {
        public const string resourceName = "SlotPrefab"; // should rename everything to InstancePrefab
        public ItemInstance containedItem;
        public SlotInstance(GameObject parent) : base(resourceName, parent)
        {

        }
    }

}
