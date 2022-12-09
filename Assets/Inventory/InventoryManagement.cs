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
    [SerializeField] private Button _closeRoomInventoryButton;
    [SerializeField] private Button _playerInventoryButton;

    [SerializeField] private Canvas _playerInventoryCanvas;

    [SerializeField] public RectTransform playerInventoryPanel;
    [SerializeField] public RectTransform roomInventoryScrollView;
    [SerializeField] public Mask scrollviewMask;
    [SerializeField] public ScrollRect scrollRect;
    //  [SerializeField] public List<Button> RoomInventoryButtons;
    [SerializeField] public List<RoomContainerScript> RoomContainers;

    [SerializeField] public Canvas RoomItemCanvas;
    [SerializeField] public float moveSpeed;

    [Inject] private GameStateManager _gameState;
    [Inject] private NewRayCaster _raycasts;
    [Inject] private NewInputManager input;
    [Inject] private GlobalTick _globalTick;
    [Inject] private ClientCalls _clientCalls;

    [SerializeField] float _limit;
    [SerializeField] float resetPosition;
    [SerializeField] bool activated;
    [SerializeField] bool superiorTo;

    private UGICollection<SlotUGI, SlotScript> _playerSlots = new();
    private UGICollection<SlotUGI, SlotScript> _roomSlots = new();
    private UGICollectionEntity<ItemUGI, ItemScript, Item> _roomItems = new();
    private UGICollectionEntity<ItemUGI, ItemScript, Item> _playerItems = new();

    private Action _filter;
    private Guid _currentRoomViewId = Guid.Empty;


    // structure :
    // Chaque item a un slot, (peut pas voir de item sans slot)
    // mais pas chaque slot ofc

    private void Awake()
    {
        // ca open les inventory Slot
        // Je pourrais juste mettre un roomName encore pis faire .where(room).select(item)

    }
    private GameObject _tracked;

    private void Start()
    {

        _closeRoomInventoryButton.AddMethod(() => this.RoomItemCanvas.enabled = false);
        _playerInventoryButton.AddMethod(()=> this._playerInventoryCanvas.enabled = !_playerInventoryCanvas.enabled);

        InitOpenContainerMethods();
        InitPlayerSlots(2);
        _globalTick.TimerTicked += OnTimerTick;

        RefreshPlayerItems();
        _currentRoomViewId = _gameState.Room.Id;
        this.RefreshRoomItems();
        //  RefreshRoomItems(_gameState.Room.Items);
        _tracked = null;
    }
    private async void Update() // ds le fond faut juste permettre de faire qqch quand le joueur est ds la piece so 
    {
        if (activated)
        {
            ClampRightwardsInventoryMovement();

        }

        // Pour pas que le player se transfer des shit entrer les items 
        if (_currentRoomViewId != _gameState.Room.Id) return;


        if (!input.Interaction) return;

        var itemRayResult = _raycasts.PointerUIRayCast(x => x.gameObject.layer == 6);
        var slotRayResult = _raycasts.PointerUIRayCast(x => x.gameObject.CompareTag("Slot"));
        var roomInventoryResult = _raycasts.PointerUIRayCast(x => x.gameObject.CompareTag("RoomInventory"));


        // Track un item
        if (input.Pressed && itemRayResult.HasFoundHit)
        {
            _tracked = itemRayResult.GameObject;
        }

        // si yavait pas ditem tracked, ca sert a rien de continuer
        if (_tracked is null) return;

        // a partir dici, ya un tracked item
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
                        _clientCalls.TransferItemOwnerShip(Guid.Empty, _gameState.Room.Id, info.TrackedItem.Id);
                        break;
                    }
                case ReleaseType.FromRoomToPlayer:
                    {
                        Debug.Log("InventoryItem released on playerSLot");
                        var excessRoomSlot = info.TrackedItem.CurrentSlot;

                        PutItemInSlot(info.TrackedItem, info.TargetSlot); // je pourrais aussi calculer le besoin de swap dans cette classe en fait vu que je fais tout le temps. 
                        SwapItemBetweenLists(info.TrackedItem);
                        _roomSlots.RemoveAndDestroy(excessRoomSlot);
                        _clientCalls.TransferItemOwnerShip(Guid.Empty, _gameState.PlayerUID, info.TrackedItem.Id);
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

    private void ClampRightwardsInventoryMovement()
    {
        if (roomInventoryScrollView.localPosition.x > _limit)
        {
            roomInventoryScrollView.localPosition = roomInventoryScrollView.localPosition.SetX(resetPosition);
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
            WasTrackedItemOwnedByPlayer = trackedItemUGI.Item.OwnerId == this._gameState.PlayerUID,
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

    // cause meme si ca switch de slot et tout, faut que ca se traduise aussi dans mes listes de UGI. Un peu chiant mais still clearer
    public void SwapItemBetweenLists(ItemUGI ugi)
    {
        bool itemOwnedByPlayer = ugi.Item.OwnerId == _gameState.PlayerUID;
        if (itemOwnedByPlayer)
        {
            ugi.Item.OwnerId = _gameState.Room.Id;
            _playerItems.RemoveReference(ugi);
            _roomItems.Add(ugi);
        }

        else
        {
            ugi.Item.OwnerId = _gameState.PlayerUID;
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
        var appearedPlayerItems = _playerItems.GetAppearedModels(_gameState.LocalPlayerDTO.Items);
        if (appearedPlayerItems.Any())
        {
            appearedPlayerItems.ForEach(x => CreateItemUGIToPlayerInventory(x)); // should this be BuilderImplementation?
            // vu que ca trouve le 1er slot, youpi ! pas besoin de faire de weird shit !
        }

        var disappearedItemsUgis = _playerItems.GetDisappearedUGis(_gameState.LocalPlayerDTO.Items);
        if (disappearedItemsUgis.Any())
        {
            _playerItems.RemoveMany(disappearedItemsUgis);

        }
    }

    public void RefreshRoomItems() // Doit dlete le slot
    {
        // Je pourrais faire un constructor pour Les appear-disappear plus complexes. 

        // () => 

        var roomsItems = _gameState.Rooms.Find(x => x.Id == _currentRoomViewId).Items;
        var appearedPlayerItems = _roomItems.GetAppearedModels(roomsItems);
        if (appearedPlayerItems.Any())
        {
            appearedPlayerItems.ForEach(x => CreateItemToRoomInventory(x)); // ici pas de builder pcq ca trouve le first

        }

        var disappearedItemsUgis = _roomItems.GetDisappearedUGis(roomsItems); // en fait faudrait deleter le roomslot aussi.
        if (disappearedItemsUgis.Any())
        {
            _roomItems.RemoveMany(disappearedItemsUgis);
            var slots = disappearedItemsUgis.Select(x => x.CurrentSlot).ToList();
            _roomSlots.RemoveMany(slots);

        }
    }

    public void CreateItemUGIToPlayerInventory(Item item)
    {
        var freeSlot = _playerSlots.UGIs.FirstOrDefault(slot=> slot.containedItem == null);

        if (freeSlot is null) throw new Exception("Should find a free slot when trying to add an item to inventory.");

        var itemUGI = _playerItems.Add(new ItemUGI(freeSlot.UnityInstance, item));
        freeSlot.containedItem = itemUGI;

        //foreach (var slot in _playerSlots.UGIs) // finds first slot then adding it 
        //{
        //    bool isFreeSlot = slot.containedItem is null;
        //    if (!isFreeSlot) continue;

        //    var itemUGI = _playerItems.Add(new ItemUGI(slot.UnityInstance, item));
        //    slot.containedItem = itemUGI;
        //    return;
        //}

        //throw new Exception("Should find a free slot when trying to add an item to inventory.");
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
        // devrait être une liste de script au lieu de boutons, pis la fonction ca fait trier avec le RoomName.

        foreach (var roomContainer in RoomContainers)
        {
            
            roomContainer.Button.AddMethod(() =>
            {
                _currentRoomViewId = _gameState.Rooms.Find(x => x.Name == roomContainer.RoomName).Id;
                this.RefreshRoomItems();
                this.RoomItemCanvas.enabled = true;
            });
            //container.AddMethod(() => this.RoomItemCanvas.enabled = !RoomItemCanvas.enabled);
        }
    }
    // On va enlever le refresh pour faire un peu comme avec les logs : un CurrentFilter 
    private void OnTimerTick(object source, EventArgs e)
    {
        _globalTick.SubscribedMembers.Add(this.GetType().Name);
        RefreshPlayerItems();
        //RefreshRoomItems(_gameState.Room.Items);
    }
}
