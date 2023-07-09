using Assets.AssetLoading;
using Assets.GameLaunch;
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
    public class InventoryManager3 : StateHolderBase<GameState> // ok mes bugs viennent de lintegration avec le nouveau manager.
    {
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private InventoryStaticGameObjects _inventoryObjects;
        [SerializeField] private SlotAndItemsManager _slotManager;
        // [SerializeField] private GameLauncherAndRefresher _gameLauncherAndRefresher;
        [SerializeField] private InventoryRefreshGuard _refreshGuard;

        public RoomDTO CurrentShownRoom => _gameState.GetRoomById(CurrentInventoryShownRoomId);
        private GameState _gameState { get; set; }
        public ItemInventory TrackedItem { get; set; }
        public Guid CurrentInventoryShownRoomId { get; set; }

        public override async UniTask Initialize(GameState gameState)
        {
            _gameState = gameState;
            await InitializePlayerSlots();
            await InitializeStaticButtons();
            await this.SetRoomInventory(_gameState.Room.Name);
            _refreshGuard.IsInitialized = true;
        }

        public override async UniTask Refresh(GameState gameState)
        {
            // refactor pour faire du pick-up only
            // donc plus de drag n drop, cest tu click ca change de owner

            // Comme ca, juste player-to room, room-to-player
            // Click sur un objet qui exist pas dans le server au moins jpeux faire du error handling asp.net
            // pis juste jamais donner limpression que le player pouvait le modifier in the first place
            // pis la pas besoin de refresh quand le player click pcq yaura pas de mismatch sur cet
            // item-la specifiquement
            // 
          

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
            var freeSlots = _slotManager.GetFreePlayerSlots().First();

            var newItemInventory = await _prefabLoader.CreateInstanceOfAsync<ItemInventory>(freeSlots.gameObject);
            await newItemInventory.Initialize(freeSlots, item);
        }

        public async UniTask InitializeStaticButtons()
        {
            _inventoryObjects.PlayerInventoryButton.AddMethod(() => _inventoryObjects.PlayerInventoryCanvas.enabled = !_inventoryObjects.PlayerInventoryCanvas.enabled);
            _inventoryObjects.RoomInventoryButton.AddMethod(() => _inventoryObjects.RoomInventoryCanvas.enabled = !_inventoryObjects.RoomInventoryCanvas.enabled);
        }

        public async UniTask RefreshItems(string roomName) // player Items & rooms items
        {
            var upToDatePlayerItems = _gameState.PlayerDTO.Items.Select(x => x).ToList();
            var currentPlayerItems = _slotManager.GetPlayerItems();
            var playerResult = RefreshResult<ItemInventory, Item>.GetRefreshResult(currentPlayerItems, upToDatePlayerItems);
            await RefreshPlayerItems(playerResult);

            var upToDateRoomItems = _gameState.GetItemsInRoom(roomName);
            var currentRoomItems = _slotManager.GetRoomItems();
            var roomResult = RefreshResult<ItemInventory, Item>.GetRefreshResult(currentRoomItems, upToDateRoomItems);
            await RefreshRoomItems(roomResult);
        }

        public async UniTask RefreshPlayerItems(RefreshResult<ItemInventory, Item> refreshResult)
        {
            foreach (var item in refreshResult.AppearedEntities)
            {
                int playerSlotsCount = _slotManager.GetFreePlayerSlotCount();
                bool outOfPlayerSlots = playerSlotsCount == 0;
                if (outOfPlayerSlots) throw new Exception("Player does not have enough inventory slots");

                await this.CreateItemInNextPlayerSlot(item);
            }
            foreach (var item in refreshResult.DisappearedPrefabs)
            {
                item.Slot.DestroyItem();
            }
        }

        public async UniTask RefreshRoomItems(RefreshResult<ItemInventory, Item> refreshResult)
        {
            foreach (var item in refreshResult.DisappearedPrefabs)
            {
                // remove room slot cos rooms cant have empty slots
                _slotManager.RemoveSlot(item.Slot);
                item.Slot.DestroyItem();
                await item.Slot.DestroySlot();
            }
            foreach (var item in refreshResult.AppearedEntities)
            {
                await _slotManager.CreateNewRoomSlotAndCreateNewItem(item);
            }
        }

        public async UniTask SetRoomInventory(string otherRoomName) // ca ici puisque ca refresh ca remove
        { // la prediction de si tas mis un item dans ton inventaire de player avant
            if (_refreshGuard.MustPreventChangingRoom()) return;
            //await _refreshGuard.WaitUntilInputEndsCoroutine();
            await _refreshGuard.WaitUntilRefreshEndsCoroutine();

            if (_refreshGuard.IsInitialized) // no condition means stack overflow cos it calls all managers what this manager calls SetRoomInventory
            {
                //   await _gameLauncherAndRefresher.ForceRefreshManagers();  // 

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
