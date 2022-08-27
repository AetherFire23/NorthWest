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
    public class YesNoDialogInstance : InstanceWrapper<YesNoDialogScript>, IDialog
    {
        public bool IsShowing { get; set; }
        public DialogResult DialogResult { get; set; }
        private const string ResourceName = "YesNoDialog";

        public YesNoDialogInstance(GameObject parent, string message) : base(ResourceName, parent)
        {
            this.IsShowing = true;
            AddButtonAction(InstanceBehaviour.OkButton, OkClick);
            AddButtonAction(InstanceBehaviour.CancelButton, CancelClick);
            this.InstanceBehaviour.PanelTextComponent.text = message;
        }

        public void OkClick()
        {
            this.DialogResult = DialogResult.Ok;
            Resolve();
        }
        public void CancelClick()
        {
            this.DialogResult = DialogResult.Cancel;
            Resolve();
        }

        public void Resolve()
        {
            GameObject.Destroy(this.UnityInstance);
            this.IsShowing = false;
            Debug.Log(this.DialogResult);
        }

        public void AddButtonAction(Button button, Action action)
        {
            button.onClick.AddListener(delegate { action(); });
        }
    }
}
