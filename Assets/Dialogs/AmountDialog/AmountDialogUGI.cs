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
    public class AmountDialogUGI : InstanceWrapper<AmountDialogScript>, IDialog
    {
        public bool IsShowing { get; set; }
        public DialogResult DialogResult { get; set; }
        /// <summary>
        /// Since inputField has invisibile characters, the inputResult is trimmed on the OkClick method.
        /// </summary>
        public string InputResult { get; set; } = String.Empty;
        private const string ResourceName = "AmountDialog";

        public AmountDialogUGI(GameObject parent, string message) : base(ResourceName, parent)
        {
            this.AccessScript.PanelTextComponent.text = message;
            this.IsShowing = true;
            AddButtonAction(AccessScript.OkButton, OkClick);
            AddButtonAction(AccessScript.CancelButton, CancelClick);
        }

        public void OkClick()
        {
            this.InputResult = TrimInputField(this.AccessScript.InputFieldTextComponent.text);
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
            Debug.Log($"Resolved a dialog with this value : {this.DialogResult}, the input was : {this.InputResult}");
            Debug.Log(this.InputResult);
        }

        public void AddButtonAction(Button button, Action action)// ark
        {
            button.onClick.AddListener(delegate { action(); });
        }

        private string TrimInputField(string inputFieldText)
        {
            return inputFieldText.Trim((char)8203);
        }
    }
}
