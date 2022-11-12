using Assets.Raycasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Input_Management
{
    public class NewInputManager : ITickable
    {
        public Vector2 PointerPosition = Vector2.zero;
        public bool Pressed => TouchState == InputState.Pressed;
        public bool Held => TouchState == InputState.Held;
        public bool Released => TouchState == InputState.Released;
        public bool Interaction => TouchState != InputState.None;

        private InputState TouchState = InputState.None;
        private bool _wasPressedLastFrame = false;

        public void Tick()
        {


            bool pressed = GetPress();

            if (pressed) // if pressed, but not last frame, it is the first press. Else, it means it was held for some time
            {
                this.TouchState = !_wasPressedLastFrame
                    ? InputState.Pressed
                    : InputState.Held;
            }

            if (!pressed) // if it was not pressed, but pressed last frame, it means it was released. if not, nothing happened.
            {
                this.TouchState = _wasPressedLastFrame
                    ? InputState.Released
                    : InputState.None;
            }

            if (TouchState != InputState.None)
            {
               //Log(TouchState);
            }

            this.PointerPosition = GetPosition(); // always update position of touch. 0,0 = out of screen.
            _wasPressedLastFrame = pressed;
        }

        private bool GetPress()
        {
            bool mousePress = Mouse.current is not null ? Mouse.current.press.ReadValue() == 1 : false;
            bool touchPress = Touchscreen.current is not null ? Touchscreen.current.press.ReadValue() == 1 : false;
            return mousePress || touchPress;
        }

        private Vector2 GetPosition()
        {
            Vector2 mousePos = Mouse.current is not null ? Mouse.current.position.ReadValue() : Vector2.zero;
            Vector2 touchPos = Touchscreen.current is not null ? Touchscreen.current.position.ReadValue() : Vector2.zero;
            return mousePos + touchPos;
        }
        // SELON UNITY LE TOUCHSCREEN KEYBOARD NE MARCHERIT PAS REVERT AU ANCIEN SYSTEME ?


        private void Log(object obj)
        {
            Debug.Log(obj);
        }

        public enum InputState
        {
            None,
            Pressed,
            Held,
            Released
        }
    }
}
