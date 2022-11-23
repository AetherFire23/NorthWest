//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Zenject;
//using UnityEngine;
//using Assets.GameState_Management;
//using WebAPI.Models;
//using Cysharp.Threading.Tasks;
//using UnityEngine.UI;
//using Assets.Input_Management;
//using UnityEngine.EventSystems;
//using Assets.Raycasts.NewRaycasts;

//namespace Assets.Inventory
//{
//    public class InventoryInputHandler : ITickable // nest pas installe
//    {
//        private readonly GameStateManager _gameStateManager;
//        private readonly ClientCalls _clientCalls;
//        private readonly NewInputManager _newInputManager;
//        private readonly NewRayCaster _newRayCaster;

//        public InventoryInputHandler(GameStateManager gameStateManager, ClientCalls clientCalls, NewInputManager newInputManager, NewRayCaster newRayCaster)
//        {
//            _newRayCaster = newRayCaster;
//            _gameStateManager = gameStateManager;
//            _clientCalls = clientCalls;
//            _newInputManager = newInputManager;
//        }

//        public void Tick()
//        {
//            var rayResult = _newRayCaster.PointerUIRayCast(x => x.gameObject.layer == 6);
//            if (!rayResult.HasFoundHit) return;

//            var hitObject = rayResult.GameObject;
//            if (_newInputManager.Held)
//            {
//                Debug.Log(hitObject.gameObject.name);

//                hitObject.transform.position = _newInputManager.PointerPosition; // SetItemAtMousePosition();
//            }

//            if (_newInputManager.Released) // quand je lâche la souris
//            {
//                //  var slotsBehindMouse = UIRaycast.MouseRaycastResult(hit => hit.gameObject.tag == "Slot");

//                var slotsBehindMouse2 = _newRayCaster.PointerUIRayCast(x => x.gameObject.tag == "Slot");

//                if (slotsBehindMouse2.HasFoundHit) // Je check si j'ai trouve un slot
//                {

//                    hitObject.GetComponent<RectTransform>().SetParent(slotsBehindMouse2.GameObject.transform);



//                    Item selectedItem = hitObject.GetComponent<ItemScript>().selfWrapper.Item;
//                    bool IsOwnedByPlayer = selectedItem.OwnerId == _gameStateManager.LocalPlayerDTO.Id; // bug car je nupdate pas 
//                    bool isReleasedOnRoomSlot = slotsBehindMouse2.GameObject.GetComponent<SlotScript>().SelfWrapper.IsRoomSlot;
//                    if (IsOwnedByPlayer)
//                    {

//                        if (isReleasedOnRoomSlot)
//                        {
//                            Guid ownerId = _gameStateManager.LocalPlayerDTO.Id;
//                            Guid targetId = _gameStateManager.Room.Id; // Target cest la currentroom.
//                            Guid itemId = selectedItem.Id; // item id pas found ? 
//                            _clientCalls.TransferItemOwnerShip(ownerId, targetId, itemId).AsTask();
//                        }
//                    }

//                    else // cetait PAS un player item
//                    {
//                        if (!isReleasedOnRoomSlot) // donc un player slot
//                        {
//                            Guid ownerId = _gameStateManager.Room.Id;
//                            Guid targetId = _gameStateManager.LocalPlayerDTO.Id; // Target cest la currentroom.
//                            Guid itemId = selectedItem.Id; // item id pas found ? 
//                            _clientCalls.TransferItemOwnerShip(ownerId, targetId, itemId).AsTask();
//                            Debug.Log("shouldve changed item ownership");
//                        }
//                    }
//                }


//                hitObject.GetComponent<RectTransform>().localPosition = Vector3.zero.WithOffset(44.5f, 0, 0); // SetItemBackToSlotPosition();
//            }
//        }
//    }
//}
