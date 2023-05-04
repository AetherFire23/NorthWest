using Assets.INVENTORY3;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Shared_Resources.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInventory : PrefabScriptBase
{
    public bool IsRoomSlot { get; private set; }
    public ItemInventory Item { get; set; } = null;
    public bool HasItem => Item is not null;

    public async UniTask Initialize(bool isRoomSlot)
    {
        IsRoomSlot = isRoomSlot;
    }

    public async UniTask PlaceItemBackAtSlotPosition() // yest jamais cense avoir de bug, cest cense etre handled par le releaseContext
    {
        if (!HasItem)
        {
            Debug.LogError($" do not call{nameof(PlaceItemBackAtSlotPosition)} if it does not have an item.");
        }

        this.Item.transform.localPosition = Vector2.zero; // will probably not work.
    }

    public async UniTask InsertItemInSlot(ItemInventory item) // item should already have some prediction going on here
    {
        if (HasItem)
        {
            Debug.LogError($"You cannot place an item in a slot that contains an item {Item.name}");
        }

        //erase all item references the current items slot
        if (item.Slot is not null)
        {
            item.Slot.Item = null;
        }

        // erase all slot references in item
        item.Slot = null;

        // set all references in item to this
        item.gameObject.SetParent(this.gameObject);
        item.Slot = this;

        //set refrences in this to item
        this.Item = item;

        await this.PlaceItemBackAtSlotPosition();
    }

    public async UniTask DestroySlot()
    {
        if (HasItem)
        {
            Debug.LogError("Cannot destroy a slot that contains an item since the item will be destroyed too.");
            throw new System.Exception();
        }

        GameObject.Destroy(this.gameObject);
    }

    public void DestroyItem()
    {
        GameObject.Destroy(this.Item.gameObject);
        this.Item = null;
    }
}
