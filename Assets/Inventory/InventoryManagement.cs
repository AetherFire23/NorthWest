using Assets;
using Assets.Big_Tick_Energy;
using Assets.GameState_Management;
using Assets.Input_Management;
using Assets.Inventory.Player_Item;
using Assets.Inventory.Slot;
using Assets.Raycasts.NewRaycasts;
using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WebAPI.Models;
using Zenject;

using Item = WebAPI.Models.Item;

public class InventoryManagement : MonoBehaviour
{
    [SerializeField] public RectTransform playerInventoryPanel;
    [SerializeField] public RectTransform roomInventoryScrollView;
    [SerializeField] public Mask scrollviewMask;
    [SerializeField] public ScrollRect scrollRect;
    [SerializeField] public List<Button> RoomInventoryButtons;
    [SerializeField] public Canvas RoomItemCanvas;
    [SerializeField] public float moveSpeed;

    [Inject] private GameStateManager gameStateManager;
    [Inject] private NewRayCaster _raycasts;
    [Inject] private NewInputManager input;
    [Inject] private GlobalTick _globalTick;
    [Inject] private ClientCalls _clientCalls;

    private UGICollectionEditor<SlotUGI, SlotScript> _playerSlots = new();
    private UGICollectionEditor<SlotUGI, SlotScript> _roomSlots = new();
    private UGICollectionEditorDbKey<ItemUGI, ItemScript, Item> _roomItems = new();
    private UGICollectionEditorDbKey<ItemUGI, ItemScript, Item> _playerItems = new();

    private void Awake()
    {
        InitOpenContainerMethods();
    }
    private GameObject _tracked;

    private void Start()
    {
        _globalTick.TimerTicked += OnTimerTick;
        InitPlayerSlots(2);
        RefreshPlayerItems();
        RefreshRoomItems();
        _tracked = null;
    }
    private async void Update()
    {
        if (!input.Interaction) return;

        var itemRayResult = _raycasts.PointerUIRayCast(x => x.gameObject.layer == 6);
        var slotRayResult = _raycasts.PointerUIRayCast(x => x.gameObject.CompareTag("Slot"));
        var roomInventoryResult = _raycasts.PointerUIRayCast(x => x.gameObject.CompareTag("RoomInventory"));

        if (input.Pressed && itemRayResult.HasFoundHit)
        {
            _tracked = itemRayResult.GameObject;
        }

        if (_tracked is null) return;

        if (input.Held)
        {
            _tracked.MoveTowards(input.PointerPosition, moveSpeed);
            SetMaskAndScrollActive(false);
        }

        if (input.Released)
        {
            var info = BuildReleaseContext(_tracked, slotRayResult.GameObject, roomInventoryResult.HasFoundHit);

            switch (info.ReleaseType)
            {
                case ReleaseType.FromPlayerToPlayer:
                    {
                        Debug.Log($"{ReleaseType.FromPlayerToPlayer}");
                        PutItemInSlot(info.TrackedItem, info.TargetSlot);
                        break;
                    }
                case ReleaseType.FromPlayerToRoom:
                    {
                        Debug.Log($"{ReleaseType.FromPlayerToRoom}");
                        CreateInventorySlotAndPutItemIntoIt(info.TrackedItem);
                        SwapItemBetweenLists(info.TrackedItem);
                        _clientCalls.TransferItemOwnerShip(Guid.Empty, gameStateManager.Room.Id, info.TrackedItem.Key);
                        break;
                    }
                case ReleaseType.FromRoomToPlayer:
                    {
                        Debug.Log("InventoryItem released on playerSLot");
                        var excessRoomSlot = info.TrackedItem.CurrentSlot;

                        PutItemInSlot(info.TrackedItem, info.TargetSlot); // je pourrais aussi calculer le besoin de swap dans cette classe en fait vu que je fais tout le temps. 
                        SwapItemBetweenLists(info.TrackedItem);
                        _roomSlots.RemoveAndDestroy(excessRoomSlot);
                        _clientCalls.TransferItemOwnerShip(Guid.Empty, gameStateManager.PlayerUID, info.TrackedItem.Key);
                        break;
                    }
                case ReleaseType.FromRoomToRoom:
                    {
                        Debug.Log("Inventory item released on inv");
                        break;
                    }
                case ReleaseType.Anywhere:
                    {
                        break;
                    }
                case ReleaseType.OnOccupiedSlot:
                    {
                        break;
                    }
            }
            _tracked = null; // stops tracking so it does not interfere when doing other stuff
            SetMaskAndScrollActive(true);
            info.TrackedItem.UnityInstance.transform.localPosition = Vector3.zero.WithOffset(44.5f, 0, 0); // returns it to the parent
            Debug.Log("test");
        }
    }

    public ReleaseInfo BuildReleaseContext(GameObject trackedObject, GameObject slotBehindMouse, bool releasedOnInventory)
    {
        var trackedItemUGI = trackedObject.gameObject.GetComponent<ItemScript>().selfWrapper;
        var trackedItemSlot = trackedItemUGI.UnityInstance.GetComponentInParent<SlotScript>().SelfWrapper;
        var slotReleasedOnUGI = slotBehindMouse is null ? null : slotBehindMouse.GetComponent<SlotScript>().SelfWrapper;
        var releasedContext = new ReleaseInfo()
        {
            TrackedItem = trackedItemUGI,
            TrackedItemSlot = trackedItemSlot,
            TargetSlot = slotReleasedOnUGI,
            WasTrackedItemOwnedByPlayer = trackedItemUGI.Item.OwnerId == this.gameStateManager.PlayerUID,
            WasOverSlotOwnedByPlayer = slotBehindMouse is null ? false : !slotReleasedOnUGI.IsRoomSlot,
            WasOverRoomInventory = releasedOnInventory,
            WasOverExistingItem = slotBehindMouse is null ? false : slotReleasedOnUGI.containedItem is not null,
        };
        releasedContext.WasReleasedAnywhere = !releasedContext.WasOverRoomInventory && !releasedContext.WasOverSlotOwnedByPlayer;

        if (releasedContext.WasTrackedItemOwnedByPlayer && releasedContext.WasOverSlotOwnedByPlayer) releasedContext.ReleaseType = ReleaseType.FromPlayerToPlayer;
        if (releasedContext.WasTrackedItemOwnedByPlayer && releasedContext.WasOverRoomInventory) releasedContext.ReleaseType = ReleaseType.FromPlayerToRoom;
        if (!releasedContext.WasTrackedItemOwnedByPlayer && releasedContext.WasOverSlotOwnedByPlayer) releasedContext.ReleaseType = ReleaseType.FromRoomToPlayer;
        if (!releasedContext.WasTrackedItemOwnedByPlayer && releasedContext.WasOverRoomInventory) releasedContext.ReleaseType = ReleaseType.FromRoomToRoom;
        if (releasedContext.WasOverExistingItem) releasedContext.ReleaseType = ReleaseType.OnOccupiedSlot;
        if (releasedContext.WasReleasedAnywhere) releasedContext.ReleaseType = ReleaseType.Anywhere;

        return releasedContext;
    }

    public void FlushAllItems()
    {
        _playerItems.Clear();
        _roomItems.Clear();
        _roomSlots.Clear();
    }

    public void SetMaskAndScrollActive(bool active)
    {
        this.scrollRect.horizontal = active;
        this.scrollviewMask.enabled = active;
    }

    public void SwapItemBetweenLists(ItemUGI ugi)
    {
        bool itemOwnedByPlayer = ugi.Item.OwnerId == gameStateManager.PlayerUID;
        if (itemOwnedByPlayer)
        {
            ugi.Item.OwnerId = gameStateManager.Room.Id;
            _playerItems.RemoveReference(ugi);
            _roomItems.Add(ugi);
        }

        else
        {
            ugi.Item.OwnerId = gameStateManager.PlayerUID;
            _roomItems.RemoveReference(ugi);
            _playerItems.Add(ugi);
        }
    }

    public void PutItemInSlot(ItemUGI item, SlotUGI targetSlot)
    {
        item.CurrentSlot.containedItem = null;
        targetSlot.containedItem = item;
        item.UnityInstance.SetParent(targetSlot.UnityInstance);
        item.UnityInstance.transform.localPosition = Vector3.zero.WithOffset(44.5f, 0, 0);
    }

    public void RefreshPlayerItems() // dois reset le containedItem
    {
        var appearedPlayerItems = _playerItems.GetAppearedModels(gameStateManager.LocalPlayerDTO.Items);
        if (appearedPlayerItems.Any())
        {
            appearedPlayerItems.ForEach(x => CreateItemUGIToPlayerInventory(x)); // should this be BuilderImplementation?
            // vu que ca trouve le 1er slot, youpi ! pas besoin de faire de weird shit !
        }

        var disappearedItemsUgis = _playerItems.GetDisappearedUGis(gameStateManager.LocalPlayerDTO.Items);
        if (disappearedItemsUgis.Any())
        {
            _playerItems.RemoveMany(disappearedItemsUgis);

        }
    }

    public void RefreshRoomItems() // Doit dlete le slot
    {
        Debug.Log("Fix needed");
        var appearedPlayerItems = _roomItems.GetAppearedModels(gameStateManager.Room.Items);
        if (appearedPlayerItems.Any())
        {
            appearedPlayerItems.ForEach(x => CreateItemToRoomInventory(x)); // should this be BuilderImplementation?

        }

        var disappearedItemsUgis = _roomItems.GetDisappearedUGis(gameStateManager.Room.Items); // en fait faudrait deleter le roomslot aussi.
        if (disappearedItemsUgis.Any())
        {
            _roomItems.RemoveMany(disappearedItemsUgis);

        }
    }

    public void CreateItemUGIToPlayerInventory(Item item)
    {
        foreach (var slot in _playerSlots.UGIs) // finds first slot then adding it 
        {
            bool slotIsOccupied = slot.containedItem is null;
            if (!slotIsOccupied) continue;

            var itemUGI = _playerItems.Add(new ItemUGI(slot.UnityInstance, item));
            slot.containedItem = itemUGI;
            return;
        }

        throw new Exception("Should find a free slot when trying to add an item to inventory.");
    }

    public void CreateInventorySlotAndPutItemIntoIt(ItemUGI itemUGI)
    {
        var slot = CreateRoomSlot();
        PutItemInSlot(itemUGI, slot);
    }

    public void CreateItemToRoomInventory(Item item) // roominventoryinfini.
    {
        var slot = CreateRoomSlot();
        var newItemUgi = _roomItems.Add(new ItemUGI(slot.UnityInstance, item));
        slot.containedItem = newItemUgi;
    }

    public void InitPlayerSlots(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreatePlayerSlot();
        }
    }

    public void CreatePlayerSlot()
    {
        var ugi = _playerSlots.Add(new SlotUGI(this.playerInventoryPanel.gameObject));
    }

    public SlotUGI CreateRoomSlot() // 
    {
        var ugi = _roomSlots.Add(new SlotUGI(roomInventoryScrollView.gameObject, true));
        return ugi;
    }

    public void InitOpenContainerMethods()
    {
        foreach (var container in RoomInventoryButtons)
        {
            container.AddMethod(() => this.RoomItemCanvas.enabled = !RoomItemCanvas.enabled);
        }
    }

    private void OnTimerTick(object source, EventArgs e)
    {
        _globalTick.SubscribedMembers.Add(this.GetType().Name);
        RefreshPlayerItems();
        RefreshRoomItems();
    }
}
