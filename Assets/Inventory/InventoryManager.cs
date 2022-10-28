using Assets.Big_Tick_Energy;
using Assets.GameState_Management;
using Assets.Inventory.ItemsUGF;
using Assets.Inventory.Player_Item;
using Assets.Inventory.Slot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WebAPI.Enums;
using WebAPI.Models;
using Zenject;
namespace Assets.Inventory
{
    public class InventoryManager : IInitializable, ITickable // manages both RoomInventory and PlayerInventory
    { // Je 
        List<ItemUGI> playerItems = new List<ItemUGI>();
        List<SlotUGI> playerSlots = new List<SlotUGI>();

        List<SlotUGI> roomSlots = new List<SlotUGI>();
        List<ItemUGI> roomItems = new List<ItemUGI>();

        private readonly GameStateManager _gameStateManager;
        private readonly GlobalTick _globalTick;
        private readonly InventoryPanelUGF _inventoryPanelUGF;
        private readonly RoomInventoryUGF _roomInventoryUGF;
        public InventoryManager(InventoryPanelUGF inventoryPanelUGF,
            GameStateManager gameStateManager,
            RoomInventoryUGF roomInventoryUGF, GlobalTick globalTick)
        {
            _globalTick = globalTick;
            _inventoryPanelUGF = inventoryPanelUGF;
            _gameStateManager = gameStateManager;
            _roomInventoryUGF = roomInventoryUGF;
        }

        public void Initialize()
        {
            InitializeSlotAmount(2);
            InitializeInventoryItems();

            InitializeRoomSlotAmount(2);
            InitializeRoomInventoryItems();

            _globalTick.TimerTicked += this.OnTimerTicked;
        }

        private void OnTimerTicked(object source, EventArgs args)
        {
            _globalTick.SubscribedMembers.Add(this.GetType().Name);


            FixItemChanges(); // quest-ce que "fix" veut dire ????
        }

        public void Tick()
        {
        }

        public void InitializeSlotAmount(int numberOfSlots)
        {
            for (int i = 0; i < numberOfSlots; i++)
            {
                SlotUGI newSlot = new SlotUGI(_inventoryPanelUGF.GameObject);
                this.playerSlots.Add(newSlot);
            }
        }

        public void InitializeRoomSlotAmount(int numberOfSlots)
        {
            for (int i = 0; i < numberOfSlots; i++)
            {
                SlotUGI newSlot = new SlotUGI(_roomInventoryUGF.GameObject, true);
                this.roomSlots.Add(newSlot);
            }
        }

        //ca load les items a partir de dummy items, va falloir faire ca a partir du database
        public void InitializeInventoryItems() // se fait au debut du launch de la game 
        {
            // var apiItems = GetPlayerItems(); // api call ?
            var apiItems = _gameStateManager.LocalPlayerDTO.Items; // devrait renvoyer Item a la place pour comparer le owner 
            foreach (Item item in apiItems)
            {
                foreach (SlotUGI slot in this.playerSlots)
                {
                    bool slotIsFree = slot.containedItem == null;
                    if (slotIsFree)
                    {
                        ItemUGI newItemInstance = new ItemUGI(slot.UnityInstance, item);
                        this.playerItems.Add(newItemInstance);
                        slot.containedItem = newItemInstance;
                        break;
                    }
                }
            }
        }

        public void InitializeRoomInventoryItems()
        {
            var apiItems = _gameStateManager.Room.Items; // devrait renvoyer Item a la place pour comparer le owner 
            foreach (Item item in apiItems)
            {
                foreach (SlotUGI slot in this.roomSlots) // va avoir plein de bugs ici avec les items parce que je skip pas les index quand yer pas free
                {
                    bool slotIsFree = slot.containedItem == null;
                    if (slotIsFree)
                    {
                        ItemUGI newItemInstance = new ItemUGI(slot.UnityInstance, item);
                        this.roomItems.Add(newItemInstance);
                        slot.containedItem = newItemInstance;
                        break;
                    }
                }
            }
        }

        public void FixItemChanges()
        {
            List<Item> allItems = new();
            allItems.AddRange(this.playerItems.Select(x => x.Item));
            allItems.AddRange(this.roomItems.Select(x => x.Item));

            //BUG ICI (sais pas si va criss deuqoi en fait)
            // Je dedouble litem on dirait 
            bool containsNewOrLessItems = allItems.Count != _gameStateManager.Room.Items.Count + _gameStateManager.LocalPlayerDTO.Items.Count;

            if (containsNewOrLessItems)
            {
                DeleteAndReinitializeItems();

            }

            var gameStateItems = _gameStateManager.Room.Items.Union(_gameStateManager.LocalPlayerDTO.Items).ToList();
            var currentShownItems = this.playerItems.Select(x => x.Item).Union(this.roomItems.Select(x => x.Item)).ToList();


            // check pour un changement de owner
            foreach (var item in gameStateItems)
            {
                foreach (var item2 in currentShownItems)
                {
                    if (item.Id == item2.Id)
                    {
                        if (item.OwnerId != item2.OwnerId) // ici
                        {
                            DeleteAndReinitializeItems();
                        }
                    }
                }
            }



        }

        public void DeleteAndReinitializeItems()
        {
            var allItems = this.playerItems.Union(this.roomItems).ToList();
            foreach (var item in allItems)
            {
                item.UnityInstance.SelfDestroy();

            }

            this.roomSlots.ForEach(x=> x.containedItem = null);
            this.playerSlots.ForEach(x => x.containedItem = null);

            this.roomItems.Clear();
            this.playerItems.Clear();

            this.InitializeInventoryItems();
            this.InitializeRoomInventoryItems();
        }
    }
}
