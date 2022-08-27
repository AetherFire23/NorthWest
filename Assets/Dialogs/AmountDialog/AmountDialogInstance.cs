using Assets.Dialogs.New_System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Dialogs
{
    public class AmountDialogInstance : InstanceWrapper<AmountDialogScript>, IDialog
    {
        public bool IsShowing { get; set; }
        public DialogResult DialogResult { get; set; }
        public string InputResult { get; set; } = String.Empty;
        private const string ResourceName = "AmountDialog";

        public AmountDialogInstance(GameObject parent, string message) : base(ResourceName, parent)
        {
            this.IsShowing = true;
            AddButtonAction(InstanceBehaviour.OkButton, OkClick);
            AddButtonAction(InstanceBehaviour.CancelButton, CancelClick);
        }

        public void OkClick()
        {
            this.InputResult = TrimInputField(this.InstanceBehaviour.InputFieldTextComponent.text);
            this.DialogResult = DialogResult.Ok;
            Resolve();
        }

        public void CancelClick()
        {
            this.InputResult = "Input was canceled";

            this.DialogResult = DialogResult.Cancel;
            Resolve();
        }

        public void Resolve()
        {
            GameObject.Destroy(this.UnityInstance);
            this.IsShowing = false;
            Debug.Log(this.DialogResult);
            Debug.Log(this.InputResult);
        }

        public void AddButtonAction(Button button, Action action)
        {
            button.onClick.AddListener(delegate { action(); });
        }

        private string TrimInputField(string inputFieldText)
        {
            return inputFieldText.Trim((char)8203);
        }
    }
}
