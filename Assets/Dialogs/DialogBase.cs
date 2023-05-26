﻿using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Dialogs.DIALOGSREFACTOR
{
    public class DialogBase : PrefabScriptBase
    {
        public void SetPosition(float x, float y)
        {
            this.transform.position = new Vector3(x, y);
        }

        private bool _resolved { get; set; } = false; // must start as false
        public DialogResult DialogResult { get; set; }
        protected async UniTask ResolveDialog(DialogResult result)
        {
            _resolved = true;
            DialogResult = result;
        }

        public async UniTask WaitForResolveCoroutine()
        {
            bool resolved = false;
            while (!resolved)
            {
                await UniTask.DelayFrame(25);
                resolved = _resolved;
                 Debug.Log($"iswaiting : {resolved}");

                await UniTask.Yield();
            }
        }
        public async UniTask Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }

        public async UniTask ConfigureResolveButton(Button button, DialogResult result)
        {
            button.AddTaskFunc(async () => await  this.ResolveDialog(result));
        }
    }
}
