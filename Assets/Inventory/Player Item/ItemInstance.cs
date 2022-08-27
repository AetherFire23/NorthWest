using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Inventory.Player_Item
{
    public class ItemInstance : InstanceWrapper<ItemScript>
    {
        public const string resourceName = "ItemInstance";
        public int ItemId;

        public ItemInstance(GameObject parent, int itemId) : base(resourceName, parent)
        {
            ItemId = itemId;
            SetTextureFromId(itemId);
        }

        public void SetTextureFromId(int itemid)
        {
            switch(itemid)
            {
                case 0:
                    {
                        Sprite wrenchSprite = Resources.Load<Sprite>("WrenchSprite");
                        EmptySpriteEx(wrenchSprite, nameof(wrenchSprite));

                        this.UnityInstance.GetComponent<Image>().sprite = wrenchSprite;
                        break;
                    }
            }
        }

        public void EmptySpriteEx(Sprite sprite, string name)
        {
            if(sprite == null)
            {
                throw new NotImplementedException($"The sprite you tried to load {name} was null. Check resources names.");
            }
        }
    }
}