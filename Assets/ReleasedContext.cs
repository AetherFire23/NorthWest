using Assets.Inventory.Player_Item;
using Assets.Inventory.Slot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class ReleaseInfo
    {
        // Items
        public SlotUGI TrackedItemSlot { get; set; }
        public ItemUGI TrackedItem { get; set; }
        public SlotUGI TargetSlot { get; set; }

        public ReleaseType ReleaseType { get; set; }

        // OwnerInfo
        public bool WasTrackedItemOwnedByPlayer { get; set; }
        public bool WasTrackedItemSlotOwnedByPlayer { get; set; }

        // ReleaseInfo
        public bool WasOverSlotOwnedByPlayer { get; set; }

        public bool WasOverRoomInventory { get; set; }
        public bool WasOverExistingItem { get; set; }
        public bool WasReleasedAnywhere { get; set; }
    }
}
