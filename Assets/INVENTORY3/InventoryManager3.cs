using Assets.AssetLoading;
using Assets.GameLaunch;
using Assets.GameState_Management;
using Assets.HttpStuff;
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
    public class InventoryManager3 : MonoBehaviour, IStartupBehavior, IRefreshable
    {
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private InventoryStaticGameObjects _inventoryObjects;
        [SerializeField] private Calls _calls;
        [SerializeField] private SlotAndItemsManager _slotManager;
        [SerializeField] private GameLauncherAndRefresher _gameLauncherAndRefresher;
        [SerializeField]  private InventoryRefreshGuard _refreshGuard;

        public RoomDTO CurrentShownRoom => _gameState.GetRoomById(CurrentInventoryShownRoomId);
        private GameState _gameState { get; set; }
        public ItemInventory TrackedItem { get; set; }
        public Guid CurrentInventoryShownRoomId { get; set; }

        // guard
        //private bool _mustWaitForNextGameState { get; set; } = false;
        //private bool _isInitialized = false;
        //private bool _isRefreshing = false;
        //private bool _isSwitchingRoomInventory { get; set; } = false;

        //private bool _isTracking => TrackedItem != null;

        public async UniTask Initialize(GameState gameState)
        {
            _gameState = gameState;
            await InitializePlayerSlots();
            await InitializeStaticButtons();
            await this.SetRoomInventory(_gameState.Room.Name);
            _refreshGuard.IsInitialized = true;
        }

        // ya une idee que pour le refresh, faut pas que le code de manipulation ditems se fasse en meme temps que le refresh et vice versa.
        // Donc cest pour ca les coroutines qui attendent refresh et input.
        // 
        public async UniTask Refresh(GameState gameState)
        {
            if (_refreshGuard.MustPreventRefresh()) return;

            _refreshGuard.IsRefreshing = true;

            _gameState = gameState;

            await RefreshItems(CurrentShownRoom.Name);
            _refreshGuard.IsRefreshing = false;
        }

        public async UniTask InitializePlayerSlots()
        {
            for (int i = 0; i < 2; i++)
            {
                await _slotManager.CreateEmptyPlayerSlot();
            }
        }

        public async UniTask CreateItemInNextPlayerSlot(Item item) //
        {
            foreach (var slot in _slotManager.GetPlayerSlots())
            {
                if (slot.HasItem) continue;

                var newItemInventory = await _prefabLoader.CreateInstanceOfAsync<ItemInventory>(slot.gameObject);
                await newItemInventory.Initialize(slot, item);
                return;
            }
            await this.RefreshItems(CurrentShownRoom.Name);
            Debug.LogError("You cannot hold more that x items");
        }

        public async UniTask InitializeStaticButtons()
        {
            _inventoryObjects.PlayerInventoryButton.AddMethod(() => _inventoryObjects.PlayerInventoryCanvas.enabled = !_inventoryObjects.PlayerInventoryCanvas.enabled);
            _inventoryObjects.RoomInventoryButton.AddMethod(() => _inventoryObjects.RoomInventoryCanvas.enabled = !_inventoryObjects.RoomInventoryCanvas.enabled);
        }

        public async UniTask RefreshItems(string roomName) // player Items & rooms items
        {
            var upToDatePlayerItems = _gameState.PlayerDTO.Items.Select(x => x).ToList();
            var playerResult = RefreshResult<ItemInventory, Item>.GetRefreshResult(_slotManager.GetPlayerItems(), upToDatePlayerItems);
            await RefreshPlayerItems(playerResult);


            var upToDateRoomItems = _gameState.GetItemsInRoom(roomName);
            var roomResult = RefreshResult<ItemInventory, Item>.GetRefreshResult(_slotManager.GetRoomItems(), upToDateRoomItems);
            await RefreshRoomItems(roomResult);
        }

        public async UniTask RefreshPlayerItems(RefreshResult<ItemInventory, Item> refreshResult)
        {
            foreach (var item in refreshResult.Appeared)
            {
                bool outOfPlayerSlots = _slotManager.GetPlayerSlots().Count == 0;
                if (outOfPlayerSlots) throw new Exception("Player does not have enough inventory slots");

                await this.CreateItemInNextPlayerSlot(item);
            }
            foreach (var item in refreshResult.Disappeared)
            {
                item.Slot.DestroyItem();
            }
        }

        public async UniTask RefreshRoomItems(RefreshResult<ItemInventory, Item> refreshResult)
        {
            foreach (var item in refreshResult.Disappeared)
            {
                _slotManager.RemoveSlot(item.Slot);
                item.Slot.DestroyItem();
                await item.Slot.DestroySlot();
            }
            foreach (var item in refreshResult.Appeared)
            {
                await _slotManager.CreateNewRoomSlotAndCreateNewItem(item);
            }
        }

        public async UniTask SetRoomInventory(string otherRoomName) // ca ici puisque ca refresh ca remove
        { // la prediction de si tas mis un item dans ton inventaire de player avant
            if (_refreshGuard.MustPreventChangingRoom()) return;
            await _refreshGuard.WaitUntilInputEndsCoroutine();
            await _refreshGuard.WaitUntilRefreshEndsCoroutine();

            if (_refreshGuard.IsInitialized) // no condition means stack overflow cos it calls all managers what this manager calls SetRoomInventory
            {
                await _gameLauncherAndRefresher.ForceRefreshManagers();

            }
            _refreshGuard.IsSwitchingRoomInventory = true;

            RoomDTO room = _gameState.GetRoomByName(otherRoomName);
            CurrentInventoryShownRoomId = room.Id;
            _inventoryObjects.RoomNameText.text = room.Name;
            await this.RefreshItems(room.Name);
            _inventoryObjects.RoomInventoryCanvas.enabled = true;
            _refreshGuard.IsSwitchingRoomInventory = false;
        }
    }
}
