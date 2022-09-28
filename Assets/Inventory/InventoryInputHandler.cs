using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using Assets.GameState_Management;
using WebAPI.Models;
using Cysharp.Threading.Tasks;

namespace Assets.Inventory
{
    public class InventoryInputHandler : ITickable // nest pas installe
    {
        private readonly GameStateManager _gameStateManager;
        private readonly ClientCalls _clientCalls;
        public InventoryInputHandler(GameStateManager gameStateManager, ClientCalls clientCalls)
        {
            _gameStateManager = gameStateManager;
            _clientCalls = clientCalls;
        }

        public void Tick()
        {
            var rayResult = UIRaycast.MouseRaycastResult(hit => hit.gameObject.layer == 6); // 6 = item layer
            if (!rayResult.HasFoundHit) return;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(rayResult.GameObject.name);
            }

            // Devrait override le layer quand yer picked up pour aller par-dessus toute 
            // solution je pense c'était au niveau de Unity et des Canvas 
            if (Input.GetMouseButton(0))
            {
                Debug.Log(rayResult.GameObject.name);

                rayResult.GameObject.transform.position = Input.mousePosition; // SetItemAtMousePosition();
            }

            if (Input.GetMouseButtonUp(0)) // quand je lâche la souris
            {
                //trouver la slote et set le parent, set la position
                var slotsBehindMouse = UIRaycast.MouseRaycastResult(hit => hit.gameObject.tag == "Slot");

                if (slotsBehindMouse.HasFoundHit) // Je check si j'ai trouve un slot
                {
                    //inserer condition ici pour ne pas mettre  2 items dans le meme slot. 
                    //faudrait faire en sorte que ca change le contained Item dans le slot class. 

                    //si oui, ce nouveau slot est son parent
                    rayResult.GameObject.GetComponent<RectTransform>().SetParent(slotsBehindMouse.GameObject.transform);

                    // Mais si ce nouveau slot appartient a linventaire de la piece et que litem venait du joueur, il faut avertir le serveur

                    // Donc est-ce qua la base javais pris un item du joueur ?
                    Item selectedItem = rayResult.GameObject.GetComponent<ItemScript>().selfWrapper.Item;
                    bool IsOwnedByPlayer = selectedItem.OwnerId == _gameStateManager.LocalPlayerDTO.Id; // bug car je nupdate pas 
                    bool isReleasedOnRoomSlot = slotsBehindMouse.GameObject.GetComponent<SlotScript>().SelfWrapper.IsRoomSlot;
                    if (IsOwnedByPlayer)
                    {
                        
                        if (isReleasedOnRoomSlot)
                        { // si oui, il faut avertir le serveur. 
                            // modifier litem et son ownerID ? - nah, on va check periodiquement si ya des changes I guess
                            Guid ownerId = _gameStateManager.LocalPlayerDTO.Id;
                            Guid targetId = _gameStateManager.CurrentRoom.Id; // Target cest la currentroom.
                            Guid itemId = selectedItem.Id; // item id pas found ? 
                            _clientCalls.TransferItemOwnerShip(ownerId, targetId, itemId).AsTask();
                        }
                    }

                    else // cetait PAS un player item
                    {
                        if(!isReleasedOnRoomSlot) // donc un player slot
                        {
                            Guid ownerId = _gameStateManager.CurrentRoom.Id;
                            Guid targetId = _gameStateManager.LocalPlayerDTO.Id; // Target cest la currentroom.
                            Guid itemId = selectedItem.Id; // item id pas found ? 
                            _clientCalls.TransferItemOwnerShip(ownerId, targetId, itemId).AsTask();
                            Debug.Log("shouldve changed item ownership");
                        }
                    }
                }

                // pas oublier l'inverse : 

                // peu importe ce qui se passe, on retourne litem a son parent
                rayResult.GameObject.GetComponent<RectTransform>().localPosition = Vector3.zero.WithOffset(44.5f, 0, 0); // SetItemBackToSlotPosition();

                // Comment savoir si l'objet appartenait au joueur et savoir que je veux le mettre dans la room ? 
                // Je pourrais changer ItemType a ItemModel
                // Ensuite 
            }
        }
    }
}
