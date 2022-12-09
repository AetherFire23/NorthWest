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
                await UniTask.Yield();
            }
        }

        /// <summary>
        /// Contains a reference to the DialogManager and will wait until Resolve() has been called.
        /// </summary>
        /// <returns></returns>
        public async UniTask WaitForResult() // comment faire ca clean ? // faire un singleton InputManager
        {
            bool isWaiting = true;
            while (isWaiting)
            {
                isWaiting = _dialogManager.IsWaitingForInput();
                await UniTask.Yield();
            }
        }
    }
}
