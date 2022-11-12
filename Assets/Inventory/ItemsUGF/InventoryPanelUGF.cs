using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Inventory.ItemsUGF
{
    public class InventoryPanelUGF : UGFWrapper
    {
        private readonly InventoryScript _inventoryPanelScript;
        public InventoryPanelUGF(InventoryScript inventoryScript) : base( inventoryScript)
        {
            _inventoryPanelScript = inventoryScript;
        }
    }
}
