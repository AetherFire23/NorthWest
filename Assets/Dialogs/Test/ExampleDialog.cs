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
    public class ExampleDialog : InstanceWrapper<ExampleDialogScript>, IDialog
    {
        public bool IsShowing { get; set; }
        public DialogResult DialogResult { get; set; }
        private const string ResourceName = "ErrorIfNotInitialized";

        public ExampleDialog(GameObject parent, string message) : base(ResourceName, parent)
        {
            this.IsShowing = true;

        }

        public void WhateverClick()
        {
            Resolve();
        }

        public void Resolve()
        {
            this.DialogResult = DialogResult.Ok;
            GameObject.Destroy(this.UnityInstance);
            this.IsShowing = false;
        }

        public void AddButtonAction(Button button, Action action)
        {
            button.onClick.AddListener(delegate { action(); });
        }
    }
}
