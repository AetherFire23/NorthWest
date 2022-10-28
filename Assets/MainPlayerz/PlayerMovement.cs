//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Zenject;
//using UnityEngine;

//public class PlayerMovement : ITickable, IFixedTickable
//{
//    private PlayerInputState _inputState;
//    private PlayerScript _playerFacade;
//    private float _movespeed { get; set; } = 3;
//
//    public PlayerMovement(PlayerInputState inputState, PlayerScript facade)
//    {
//        _inputState = inputState;
//        _playerFacade = facade;
//    }

//    public void Tick()
//    {
//        if (_inputState.IsMouseClicked)
//        {
//            TurnLeftOrRight();
//        }
//    }

//    public void FixedTick()
//    {
//        // move player rwith rigid body
//        Vector2 moveAmount = Vector3.MoveTowards(_playerFacade.RigidBody.position, _inputState.TargetPosition, Time.deltaTime * _movespeed);
//      //  Vector2 moveAmount = Vector3.Lerp(_playerFacade.RigidBody.position, _inputState.TargetPosition, Time.deltaTime * _movespeed);
        
//        _playerFacade.RigidBody.MovePosition(moveAmount); // use this to move player
        
//    }
//    public void TurnLeftOrRight() // faire * -1 
//    {
//        //float localScaleValue = _playerFacade.Transform.localScale.x;
//        //if (_inputState.IsMovingLeft)
//        //{
//        //    float negative = localScaleValue * -1;
//        //    _playerFacade.Transform.localScale = _playerFacade.Transform.localScale.SetX(negative);

//        //}
//        //if (_inputState.IsMovingRight)
//        //{
//        //    float positive = localScaleValue * 1;
//        //    _playerFacade.Transform.localScale = _playerFacade.Transform.localScale.SetX(positive);
//        //}
//    }
//}