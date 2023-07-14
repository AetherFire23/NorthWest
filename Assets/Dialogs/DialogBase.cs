using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Dialogs.DIALOGSREFACTOR
{
    public abstract class DialogBase : PrefabScriptBase
    {
        [SerializeField] Canvas Canvas;

        protected abstract string Name { get; }

        public bool Resolved { get; set; } = false; // must start as false
                                                    // public ObservableBool IsResolvedObservable { get; set; } = new ObservableBool(false);
        public DialogResult DialogResult { get; set; }


        public delegate UniTask OkDialogResolveHandler();
        public event OkDialogResolveHandler OnOkDialogResolve;

        public delegate UniTask CancelDialogResolveHandler();
        public event CancelDialogResolveHandler OnCancelDialogResolve;

        public void SetPosition(float x, float y)
        {
            this.transform.position = new Vector3(x, y);
        }

        /// <summary> Provokes the dialog handlers</summary>
        public async UniTask ResolveDialog(DialogResult result, bool canInvokeOnSameResult = true)
        {
            Resolved = true;

            if (!canInvokeOnSameResult && DialogResult.Equals(result)) return;

            DialogResult = result;

            switch (DialogResult)
            {
                case DialogResult.Ok:
                    {
                        if (OnOkDialogResolve is null)
                        {
                            Debug.Log("evocation list was 0 for list!");
                            break;
                        }
                        await OnOkDialogResolve.Invoke();
                        break;
                    }
                case DialogResult.Cancel:
                    {
                        await OnCancelDialogResolve.Invoke();
                        break;
                    }
            }
        }

        public async UniTask WaitForResolveCoroutine()
        {
            bool resolved = false;
            while (!resolved)
            {
                await UniTask.DelayFrame(25);
                resolved = Resolved;
                Debug.Log($"iswaiting : {resolved}, {Name}");

                await UniTask.Yield();
            }
        }

        public async UniTask Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }

        public async UniTask ConfigureResolveButton(Button button, DialogResult result)
        {
            button.AddTaskFunc(async () => await this.ResolveDialog(result));
        }

        public void ToggleCanvas()
        {
            if (Canvas is null)
            {
                throw new Exception($"Tried to toggle a null canvas on dialog - {Name}");
            }
            Canvas.enabled = !Canvas.enabled;
        }

        public void ToggleCanvas(bool value)
        {
            if (Canvas is null)
            {
                throw new Exception($"Tried to toggle a null canvas on dialog - {Name}");
            }
            Canvas.enabled = value;
        }

        public async UniTask ToggleCanvasAsync(bool value)
        {
            ToggleCanvas(value);
        }
    }
}
