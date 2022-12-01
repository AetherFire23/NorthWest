using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Dialogs.New_System
{
    public interface IDialog
    {
        public bool IsShowing { get; set; } // must start as false
        public DialogResult DialogResult { get; set; }
        public void Resolve();
        public void AddButtonAction(Button button, Action action);
    }
}
