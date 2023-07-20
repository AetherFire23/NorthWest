using Assets.AssetLoading;
using Assets.EntityRefresh;
using Assets.GameLaunch.BaseLauncherScratch;
using Assets.GameState_Management;
using Assets.Utils;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.INVENTORY3
{
    // les events sont gérés 1 à la fois ce qui est wicked sick faque
    // 0 async issues
    public class InventoryManager3 : StateHolderBase<GameState> // ok mes bugs viennent de lintegration avec le nouveau manager.
    {
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private InventoryStaticGameObjects _inventoryObjects;

        // all items are queried from the slots that theyre in so no need to add items to the inventory.
        [SerializeField] private SlotAndItemsManager _slotManager;
        [SerializeField] private InventoryRefreshGuard _refreshGuard;

        [SerializeField] private ScratchEntityBaseRefresher<ItemInventory, Item> _playerInventoryRefresher;
        [SerializeField] private ScratchEntityBaseRefresher<ItemInventory, Item> _roomInventoryRefresher;

        public RoomDTO CurrentShownRoom => _gameState.GetRoomById(CurrentRoomInventoryShownId);
        private GameState _gameState { get; set; }
        //public ItemInventory TrackedItem { get; set; }
        public Guid CurrentRoomInventoryShownId => _gameState.GetRoomByName(_currentShownRoomName).Id;
        private string _currentShownRoomName 
        { 
            get { return _inventoryObjects.RoomNameText.text; } 
            set => _inventoryObjects.RoomNameText.text = value; 
        }
        public override async UniTask Initialize(GameState gameState)
        {
            _playerInventoryRefresher = new(CreatePlayerItem, OnDeletePlayerItem);
            _roomInventoryRefresher = new(CreateRoomItem, OnDeleteRoomItem);

            _gameState = gameState;
            _currentShownRoomName = _gameState.LocalPlayerRoom.Name;
            await InitializePlayerSlots();
            await InitializeStaticButtons();

            await RefreshInventories();
            // await this.ChangeRoomInventoryAndRefresh(_gameState.LocalPlayerRoom.Name);
            _refreshGuard.IsInitialized = true;
        }

        public override async UniTask Refresh(GameState gameState)
        {
            if (_refreshGuard.MustPreventRefresh()) return;
            _refreshGuard.IsRefreshing = true;

            _gameState = gameState;
            await RefreshInventories();

            _refreshGuard.IsRefreshing = false;
        }

        //public async UniTask InitializePlayerSlots()
        //    => await UniTask.WhenAll(Enumerable.Range(0, 2).Select(_ => _slotManager.CreateEmptyPlayerSlot()));

        public async UniTask InitializePlayerSlots()
        {
            for (int i = 0; i < 2; i++)
            {
                await _slotManager.CreateEmptyPlayerSlot();
            }
        }

        public async UniTask<ItemInventory> CreateItemInNextPlayerSlot(Item item) //
        {
            var firstFreeSlot = _slotManager.GetFreePlayerSlots().First();
            var newItemInventory = await _prefabLoader.CreateInstanceOfAsync<ItemInventory>(firstFreeSlot.gameObject);
            await newItemInventory.Initialize(firstFreeSlot, item);
            return newItemInventory;
        }

        public async UniTask InitializeStaticButtons()
        {
            _inventoryObjects.PlayerInventoryButton.AddMethod(() => _inventoryObjects.PlayerInventoryCanvas.enabled = !_inventoryObjects.PlayerInventoryCanvas.enabled);
            _inventoryObjects.RoomInventoryButton.AddMethod(() => _inventoryObjects.RoomInventoryCanvas.enabled = !_inventoryObjects.RoomInventoryCanvas.enabled);
        }

        public async UniTask RefreshInventories()
        {
            await RefreshPlayerItems();
            await RefreshRoomItems();
        }

        public async UniTask RefreshPlayerItems()
        {
            var upToDatePlayerItems = _gameState.PlayerDTO.Items;
            await _playerInventoryRefresher.RefreshEntities(upToDatePlayerItems);
        }

        public async UniTask<ItemInventory> CreatePlayerItem(Item appearedItem)
        {
            int playerSlotsCount = _slotManager.GetFreePlayerSlotCount();
            bool outOfPlayerSlots = playerSlotsCount is 0;
            if (outOfPlayerSlots) throw new Exception("Player does not have enough inventory slots so validate plz");

            var itemInventory = await CreateItemInNextPlayerSlot(appearedItem);
            return itemInventory;
        }

        public async UniTask OnDeletePlayerItem(ItemInventory disappearedItem)
        {
            disappearedItem.Slot.DestroyItem();
        }

        public async UniTask RefreshRoomItems()
        {
            var upToDateRoomItems = _gameState.GetItemsInRoom(_currentShownRoomName);
            await _roomInventoryRefresher.RefreshEntities(upToDateRoomItems);
        }

        public async UniTask<ItemInventory> CreateRoomItem(Item appearedItem)
        {
            var itemInventory = await _slotManager.CreateNewRoomSlotAndCreateNewItem(appearedItem);
            return itemInventory;
        }

        public async UniTask OnDeleteRoomItem(ItemInventory disappearedItem)
        {
            // remove room slot cos rooms cant have empty slots
            _slotManager.RemoveSlot(disappearedItem.Slot);
            disappearedItem.Slot.DestroyItem();
            await disappearedItem.Slot.DestroySlot();
        }

        public async UniTask ChangeRoomInventoryAndRefresh(string otherRoomName) // ca ici puisque ca refresh ca remove
        {

            if (_refreshGuard.MustPreventChangingRoom()) return;
            await _refreshGuard.WaitUntilRefreshEndsCoroutine();

            _refreshGuard.IsSwitchingRoomInventory = true;

            _currentShownRoomName = otherRoomName;

            await RefreshInventories();

            _inventoryObjects.RoomInventoryCanvas.enabled = true;
            _refreshGuard.IsSwitchingRoomInventory = false;
        }
    }
}
