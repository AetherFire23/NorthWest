using Assets.Inventory.Models;
using Assets.Inventory.Player_Item;
using Assets.Inventory.Slot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
namespace Assets.Inventory
{
    public class InventoryManager  : IInitializable
    {
        List<ItemInstance> playerItems = new List<ItemInstance>();
        List<SlotInstance> playerSlots = new List<SlotInstance>(); 

        private readonly InventoryScript inventoryPanelScript;
        private readonly GameObject inventoryPanelContainer;


        public InventoryManager(InventoryScript inventoryScript)
        {
            inventoryPanelScript = inventoryScript;
            inventoryPanelContainer = inventoryPanelScript.gameObject;
        }

        public void Initialize()
        {
            InitializeSlotAmount(2);
            InitializeInventoryItems();
        }

        public void InitializeSlotAmount(int numberOfSlots)
        {
            for (int i = 0; i < numberOfSlots; i++)
            {
                SlotInstance newSlot = new SlotInstance(inventoryPanelContainer);
                // je pourrais quasimenet ajouter les slots dans le constructor
                this.playerSlots.Add(newSlot);
            }
        }

        public void InitializeInventoryItems() // se fait au debut du launch de la game 
        {
            var apiItems = GetPlayerItems();
            foreach (ItemModel itemModel in apiItems)
            {
                foreach(SlotInstance slot in this.playerSlots)
                {
                    bool slotIsFree = slot.containedItem == null;
                    if(slotIsFree)
                    {
                        ItemInstance newItemInstance = new ItemInstance(slot.UnityInstance, itemModel.ResourceId);
                        this.playerItems.Add(newItemInstance);
                        slot.containedItem = newItemInstance;
                        break;
                    }

                }
                // mon idee cest que les slot vont avoir leur position calculee automatic i guess et le panel
                //resized avec eux,
                //mais par contre le Item va etre agnostic de sa slot ou de sa position.
                // la slot va contenir litem. 
            }

            // c p-t le slot qui devrait avoir litem, litem c juste un objet abstrait. 
            // donc le slot a litem, et cest le slot que tu bouges ? 
            // nah litem doit etre un gameobject pour que le slot rest visible et interactible, au pire y peut etre invisible mais y doit exister 

            //design decision : faire 2 slot hardcode
        }



   

        public void CreateItemInstance(ItemModel itemModel)
        {
            // le parent devrait etre le Slot 
            // jai besion du originalPosition du item. Il doit etre fait dans le input handler I guess ? 
            // ItemInstance newItemInstance = new ItemInstance(inventoryPanel,itemModel.ResourceId );
        }

        public List<ItemModel> GetPlayerItems() //api call
        {
            var dummyItems = new List<ItemModel>();

            dummyItems.Add(new ItemModel()
            {
                Id = new Guid(),
                ResourceId = 0,
                Name = "Wrench"
            });

            dummyItems.Add(new ItemModel()
            {
                Id = new Guid(),
                ResourceId = 0,
                Name = "Wrench"
            });
            return dummyItems;
        }

    }
}
