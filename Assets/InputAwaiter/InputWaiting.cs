using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.InputAwaiter
{
    public class InputWaiting 
    {
        private readonly DialogManager _dialogManager;
        // private readonly InputWaiting _inputWaiting;

        // InputWaiting inputWaiting,
        //_inputWaiting = inputWaiting;
        public InputWaiting(DialogManager dialogManager)
        {
            _dialogManager = dialogManager;
        }

        public async UniTask ForKeyPress(KeyCode keycode)
        {
            Func<KeyCode, bool> input = Input.GetKey;
            while (input(keycode) == false)
            {
                Debug.Log($"{nameof(input)}");
                await UniTask.Yield();
            }
        }

        public async UniTask WaitForNameSelection()
        {
            bool isWaiting = true;
            while (isWaiting)
            {
                // isWaiting = _messageManager.IsWaitingForInput(); // need to use new dialog system
                await UniTask.Yield();
            }
        }

        // await _inputManager.WaitForDialogInput()
        public async UniTask WaitForResult() // comment faire ca clean ? // faire un singleton InputManager
        {
            bool isWaiting = true;
            while (isWaiting)
            {
                isWaiting = _dialogManager.IsWaitingForInput();

                Debug.Log($"State of waiting for input : {isWaiting}");
                //await UniTask.Delay(100); // faudrait tester avec du delay 
                await UniTask.Yield();
            }
        }
    }
}
