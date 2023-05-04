//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using System;
//using Zenject;
//using Assets.Big_Tick_Energy;
//using Assets.GameState_Management;

//public class PlayerScript : MonoBehaviour
//{
//    [SerializeField] public Transform Transform;
//    [SerializeField] public Rigidbody2D RigidBody;
//    [SerializeField] public TextMeshProUGUI PlayerTextComponent;


//    public Vector3 Position => this.transform.position;
//    public Vector3 PlayerScreenPosition => Camera.main.WorldToScreenPoint(Position);
//    private SimpleTimer _timer = new SimpleTimer(3f);
//    private GameStateManager _gameStateManager;
//   // private GlobalTick _globalTick;

//    private void Start()
//    {
//        this.PlayerTextComponent.text = _gameStateManager.LocalPlayerDTO.Name;
//        //_globalTick.TimerTicked += this.OnTimerTick;
//        //_globalTick.SubscribedMembers.Add(nameof(PlayerScript));
//    }

//    private void OnDestroy()
//    {
//        //_globalTick.TimerTicked -= this.OnTimerTick;
//    }

//    //private void OnEnable() // A utiliser de preference si le gameobject peut disappear.
//    //{
//    //    _globalTick.TimerTicked += this.OnTimerTick;

//    //}
//    //private void OnDisable()
//    //{
//    //    _globalTick.TimerTicked -= this.OnTimerTick;
//    //}

//    void Update()
//    {
//        SetTextPositionOverHead();
//    }

//    public void SetTextPositionOverHead()
//    {
//        PlayerTextComponent.transform.position = PlayerScreenPosition.WithOffset(0, 45, 0);
//    }

//    public void PlacePlayerCenterRoom(RoomScript roomScript)
//    {
//        ChangePlayerPositionWithCamera(roomScript.gameObject.transform.position);
//    }

//    private void ChangePlayerPositionWithCamera(Vector2 newPosition)
//    {
//        this.gameObject.transform.position = newPosition;
//        Camera.main.gameObject.transform.position = this.Position.WithOffset(0, 0, -10); // stuck there tough 
//    }

//    //private void OnTimerTick(object source, EventArgs e)
//    //{
//    //                    _globalTick.SubscribedMembers.Add(this.GetType().Name);


//    //}

//    //[Inject]
//    //private void Construct(GlobalTick globalTick, GameStateManager gameStateManager)
//    //{
//    //    _gameStateManager = gameStateManager;
//    //    //_globalTick = globalTick;
//    //}
//}
