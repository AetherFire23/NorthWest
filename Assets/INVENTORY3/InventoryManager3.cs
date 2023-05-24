﻿using Assets.AssetLoading;
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
using Unity.VisualScripting;
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

        private GameState _gameState { get; set; }
        private ItemInventory _trackedItem { get; set; }
        private Guid _currentInventoryShownRoomId { get; set; }
        public RoomDTO CurrentShownRoom => _gameState.GetRoomById(_currentInventoryShownRoomId);

        public async UniTask Initialize(GameState gameState)
        {
            _gameState = gameState;

            await InitializePlayerSlots();
            await InitializeStaticButtons();
            await this.SetRoomInventory(_gameState.Room.Name);
            _isInitialized = true;
        }

        // ya une idee que pour le refresh, faut pas que le code de transfert ditems se fasse en meme temps que le refresh et vice versa.
        // Donc cest pour ca les coroutines qui sattendent lun et lautre.
        // va falloir faire une classe pour manage ca par contre cos on comprend rien pourquoi jattends des shit dans le code

        // trying something to wait for next gameState
        private bool _mustWaitForNextGameState { get; set; } = false; 
        public async UniTask Refresh(GameState gameState)
        {
            if (!_isInitialized) return;
            if (_isRefreshing) return;
            if (_isSwitchingRoomInventory)
            {
                Debug.LogError("Refresh canceled because is switching inventory");
                return;
            }
            if (_isTracking)
            {
                Debug.LogError("Rfersh canceled because of input handling");
                return;
            }

            if (_mustWaitForNextGameState)
            {
                _mustWaitForNextGameState = false;
                return;
            }

            _isRefreshing = true;

            _gameState = gameState;


            await RefreshItems(CurrentShownRoom.Name);
            _isRefreshing = false;
        }

        private bool _isInitialized = false;
        private bool _isTracking => _trackedItem != null;
        private bool _isRefreshing = false;
        public async UniTask Update()
        {
            if (!_isInitialized) return;
            if (_isRefreshing) return;
            if (_isTracking) return;
            if (!Input.GetMouseButtonDown(0)) return;

   

            ItemInventory itemClick = UIRaycast.ScriptOrDefault<ItemInventory>();
            if (itemClick is null) return;

            _trackedItem = itemClick;

            await HandleItemTrackingUntilMouseReleaseCoroutine();// si la coroutine roule pendant que ca refresh ca fuck up peu 
            // Check if items were refreshed while handling ??

            await this.WaitUntilRefreshEndsCoroutine();
            ReleaseType context = await GetItemReleasedContext();
            Debug.Log(context);
            if (context == ReleaseType.Invalid)
            {
                await _trackedItem.PlaceItemBackAtSlot();
                _trackedItem = null;
                Debug.Log(" was not a valid release");
                return;
            }

            await HandleItemSwap(context);

            _trackedItem = null; // in all cases, when the button is lifted up, should stop tracking everything.
        }

        public async UniTask HandleItemSwap(ReleaseType releaseType)
        {
            // the reason why is procesingInput is only going from room-player and vice-versa
            // is because there is no http call from within-room inventories so no chance of delay.
            switch (releaseType)
            {
                case ReleaseType.FromPlayerToPlayer:
                    {
                        var targetSlot = UIRaycast.ScriptOrDefault<SlotInventory>();
                        await targetSlot.InsertItemInSlot(_trackedItem);
                        break;
                    }
                case ReleaseType.FromPlayerToRoom:// ownerShipChange
                    {
                        _mustWaitForNextGameState = true;
                        var slot = await _slotManager.CreateInventorySlotAndInsertItem(_trackedItem);
                        // For prediction
                        _trackedItem.Item.OwnerId = _currentInventoryShownRoomId;
                        await _calls.TransferItemOwnerShip(_currentInventoryShownRoomId, _trackedItem.Id);
                        break;
                    }
                case ReleaseType.FromRoomToPlayer: // ownerShipChange
                    {
                        _mustWaitForNextGameState = true;
                        var oldInventorySlot = _trackedItem.Slot;
                        var playerSlot = UIRaycast.ScriptOrDefault<SlotInventory>();
                        await playerSlot.InsertItemInSlot(_trackedItem);
                        await oldInventorySlot.DestroySlot();
                        _trackedItem.Item.OwnerId = PlayerInfo.UID;
                        await _calls.TransferItemOwnerShip(PlayerInfo.UID, _trackedItem.Id);
                        break;
                    }
                case ReleaseType.FromRoomToRoom:
                    {
                        await _trackedItem.PlaceItemBackAtSlot();
                        break;
                    }
            }
        }

        // what a mess though. Should check for invalids first maybe
        private async UniTask<ReleaseType> GetItemReleasedContext()
        {
            if (await IsInvalidRelease()) return ReleaseType.Invalid;


            bool isOverRoomInventory = UIRaycast.TagExists("RoomInventory");
            if (isOverRoomInventory)
            {
                if (_trackedItem.IsPlayerOwned) return ReleaseType.FromPlayerToRoom;
                else return ReleaseType.FromRoomToRoom;
            }

            else // player was already checked so it must work
            {
                if (_trackedItem.IsPlayerOwned) return ReleaseType.FromPlayerToPlayer;
                else return ReleaseType.FromRoomToPlayer;
            }
        }

        private async UniTask<bool> IsInvalidRelease()
        {
            // check if on another item
            // should I make a class ????
            var otherItems = UIRaycast.MouseRaycastResult(x => x.gameObject.GetComponent<ItemInventory>() is not null)
                .raycastResults.Select(x => x.gameObject.GetComponent<ItemInventory>())
                .Where(x => x != _trackedItem);

            var isOverAnotherItem = otherItems
                .Any();

            if (isOverAnotherItem) return true;

            // check if the slot that its been released on is the same item thats being tracked
            var slotBehind = UIRaycast.ScriptOrDefault<SlotInventory>();
            if (slotBehind == _trackedItem.Slot) return true;

            // Check if the item is being dropped on a SLOT (that contains an item
            // && operator is important because if I just check for .HasItem when dragging on inventory,
            // you get a null ref exception
            if (slotBehind != null && slotBehind.HasItem) return true; // 



            // check if release is either over room or player, if none, it is invalid (anywhere else on the screen)
            bool isOverRoomInventory = UIRaycast.TagExists("RoomInventory");
            bool isOverPlayerSlot = UIRaycast.ScriptOrDefault<SlotInventory>() is not null;
            bool eitherOnPlayerOrInventory = isOverRoomInventory || isOverPlayerSlot;
            if (!eitherOnPlayerOrInventory) return true;

            return false;
        }

        // les shits dans la coroutine ca fait peur honestly
        private async UniTask HandleItemTrackingUntilMouseReleaseCoroutine() // devrais quasiment faire 2 manager differents
        {
            // changing the tracked item parent to prevent the mask from hiding the gameObject.
            _trackedItem.transform.parent = _inventoryObjects.RoomInventoryCanvas.transform;
            while (Input.GetMouseButton(0) && !_isRefreshing)
            {
                _trackedItem.Position = Input.mousePosition;
                await UniTask.Yield();
            }
            _trackedItem.transform.SetParent(_trackedItem.Slot.gameObject.transform);
        }

        private async UniTask WaitUntilRefreshEndsCoroutine()
        {
            if (!_isRefreshing) return;

            while (_isRefreshing)
            {
                Debug.Log("Waiting for refresh before handling manipulation");
                await UniTask.Yield();
            }
        }
        private async UniTask WaitUntilInputEndsCoroutine()
        {
            while (_isTracking)
            {
                Debug.Log("Waiting Until Refresh Ends");
                await UniTask.Yield();
            }
        }

        //private async UniTask 

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

        // could probably split this furthe rbut whatever
        public async UniTask RefreshItems(string roomName) // player Items & rooms items
        {
            var upToDatePlayerItems = _gameState.PlayerDTO.Items.Select(x => x).ToList();
            var playerResult = RefreshResult<ItemInventory, Item>.GetRefreshResult(_slotManager.GetPlayerItems(), upToDatePlayerItems);
            await RefreshPlayerItems(playerResult);


            var upToDateRoomItems = await _gameState.GetItemsInRoom(roomName);
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
                // jfais tu actually dequoi pour enelver la slot du itemmanager?
                item.Slot.DestroyItem();
            }
        }

        public async UniTask RefreshRoomItems(RefreshResult<ItemInventory, Item> refreshResult)
        {
            foreach (var item in refreshResult.Appeared)
            {
                await _slotManager.CreateNewRoomSlotAndCreateNewItem(item);
            }
            foreach (var item in refreshResult.Disappeared)
            {
                _slotManager.RemoveSlot(item.Slot);
                item.Slot.DestroyItem();
                await item.Slot.DestroySlot();
            }
        }

        private bool _isSwitchingRoomInventory { get; set; } = false;
        public async UniTask SetRoomInventory(string roomName) // ca ici puisque ca refresh ca remove
        { // la prediction de si tas mis un item dans ton inventaire de player avant
            await WaitUntilInputEndsCoroutine();
            await WaitUntilRefreshEndsCoroutine();

            if (_isInitialized) // else infinite loop cos it calls all managers
            {
                await _gameLauncherAndRefresher.ForceRefreshManagers();

            }
            // si tu 1. switch item dinventaire 


            // possibilites de plein del oops infinis ici so wathc out
            _isSwitchingRoomInventory = true;




            RoomDTO room = _gameState.GetRoomByName(roomName);
            _currentInventoryShownRoomId = room.Id;
            this._inventoryObjects.RoomNameText.text = room.Name;
            await this.RefreshItems(room.Name);
            _inventoryObjects.RoomInventoryCanvas.enabled = true;

            _isSwitchingRoomInventory = false;

        }
    }
}
