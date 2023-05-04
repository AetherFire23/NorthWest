using Assets.Inventory.Player_Item;
using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Inventory.Slot
{
    public class SlotUGI : InstanceWrapper<SlotScript>
    {
        public const string resourceName = "SlotPrefab";
        public ItemUGI containedItem { get; set; }
        public bool IsRoomSlot = false;
        public SlotUGI(GameObject parent, bool isRoomSlot = false) : base(resourceName, parent)
        {
            IsRoomSlot = isRoomSlot;
            this.AccessScript.SelfWrapper = this;
        }
    }
}
