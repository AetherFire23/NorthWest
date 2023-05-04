//using Assets.GameState_Management;
//using Assets.Raycasts.NewRaycasts;
//using Assets.Tasks_On_Players;
//using Assets.Utils;
//using System.Collections;
//using System.Collections.Generic;
//using System.Net.Http;
//using TMPro;
//using UnityEngine;
//using Zenject;

//public class Tasks_On_Player : MonoBehaviour
//{
//    [Inject] NewRayCaster _rayCasts;
//    [Inject] ClientCalls _client;
//    [Inject] GameStateManager _gameState;

//    [SerializeField] RectTransform _tasksPanel;
//    [SerializeField] Canvas _canvas;
//    [SerializeField] TextMeshProUGUI PlayerNameText;
//    public UGICollection<PlayerTaskButtonUGI, PLayerTaskButtonScript> _taskButtons;

//    void Start()
//    {
//        _taskButtons = new UGICollection<PlayerTaskButtonUGI, PLayerTaskButtonScript>();
//    }

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {

//            var result = _rayCasts.PointerPhysicsRaycast(x=> x.CompareTag("Player")); // || "OtherPlayer"

//            // So that the panel is not closed when clicking on the panel but not on a player
//            var onUI = _rayCasts.PointerUIRayCast(x => x.gameObject.CompareTag("PlayerTaskPanel"));
//            if (!result.HasFoundHit && !onUI.HasFoundHit)
//            {

//                _canvas.enabled = false;
//                _taskButtons.Clear();
//                return;
//            }


//            if (_canvas.enabled) return;

            


//            // Savoir sur quoi jai click
//            var localPlayerScript = result.HitObject.GetComponent<PlayerScript>();
//            var otherCharacterScript = result.HitObject.GetComponent<OtherCharacterAS>();

//            bool isLocalPlayer = localPlayerScript is not null;
            
//            // si cest un autre joueur PAS dans la meme piece, cancel ??? Ou en tout cas on verra, jai le code !
//            if(!isLocalPlayer)
//            {
//                bool otherPlayerIsInOtherRoom = otherCharacterScript.SelfWrapper.DbModel.CurrentGameRoomId != _gameState.Room.Id;
//                if(otherPlayerIsInOtherRoom)
//                {
//                    return;
//                }
//            }
//            // a partir dici, la validation devrait etre correcte 
//            _canvas.enabled = true;

//            // set le texte
//            PlayerNameText.text = isLocalPlayer
//                ? localPlayerScript.PlayerTextComponent.text
//                : otherCharacterScript.TextOverHead.text;

            

//            var button = _taskButtons.Add(new PlayerTaskButtonUGI(_tasksPanel.gameObject, "TEST"));

//        }
//    }

//    private void CreateTasks()
//    {
//        // some condition if KillTask can be used.
//    }

//    private void TryExecuteKillTask()
//    {
//        // _client.TryExecuteKillTask();
//    }
//}
