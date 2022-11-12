using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Inventory.ItemsUGF
{
    public class RoomInventoryUGF : UGFWrapper
    {
        public RoomInventoryAS RoomInventoryAS;
        public RoomInventoryUGF(RoomInventoryAS roomInventoryAS) : base(roomInventoryAS)
        {
            RoomInventoryAS = roomInventoryAS;
        }
    }
}
