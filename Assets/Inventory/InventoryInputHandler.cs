using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace Assets.Inventory
{
    public class InventoryInputHandler : ITickable // nest pas installe
    {

        public void Tick()
        {
            var rayResult = UIRaycast.MouseRaycastResult(hit => hit.gameObject.layer == 6); // 6 = item layer
            if (!rayResult.HasFoundHit) return;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(rayResult.GameObject.name);
            }

            // Devrait override le layer quand yer picked up pour aller par-dessus toute 
            if (Input.GetMouseButton(0))
            {
                Debug.Log(rayResult.GameObject.name);

                // 
                rayResult.GameObject.transform.position = Input.mousePosition; // SetItemAtMousePosition();
                                                                               //SetSortingLayerTo
            }

            if (Input.GetMouseButtonUp(0))
            {
                //trouver la slote et set le parent
                var slotsBehindMouse = UIRaycast.MouseRaycastResult(hit => hit.gameObject.tag == "Slot"); // 6 = item layer

                if (slotsBehindMouse.HasFoundHit)
                {
                    //inserer condition ici pour ne pas mettre  2 items dans le meme slot. 
                    //faudrait faire en sorte que ca change le contained Item dans le slot class. 
                    //
                    rayResult.GameObject.GetComponent<RectTransform>().SetParent(slotsBehindMouse.GameObject.transform);
                }


                rayResult.GameObject.GetComponent<RectTransform>().localPosition = Vector3.zero.WithOffset(44.5f, 0, 0); // SetItemBackToSlotPosition();
            }
        }
    }
}
