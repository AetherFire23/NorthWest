using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    public class PlayerInputHandler : ITickable, IFixedTickable
    {
        PlayerInputState _inputState;
        PlayerScript _playerFacade;

        private bool IsMouseClicked
        {
            get
            {
                _inputState.IsMouseClicked = true;
                return Input.GetMouseButton(0);
            }
        }

        public PlayerInputHandler(PlayerScript playerFacade, PlayerInputState inputState)
        {
            _inputState = inputState;
            _playerFacade = playerFacade;
        }

        public void Tick() // remember, jaggy cos physics are 50 fps
        {

            if (IsMouseClicked)
            {
                var mouseAsWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _inputState.ClickMousePosition = mouseAsWorldPoint;
                _inputState.IsMovingLeft = _inputState.ClickMousePosition.x < _playerFacade.Position.x;
                _inputState.IsMovingRight = _inputState.ClickMousePosition.x > _playerFacade.Position.x;
                _inputState.TargetPosition = _inputState.ClickMousePosition;
            }
        }

        public void FixedTick()
        {
        }

       
    }
}