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
                // isWaiting = _messageManager.IsWaitingForInput(); // need to use new dialog system
                await UniTask.Yield();
            }
        }
    }
}
