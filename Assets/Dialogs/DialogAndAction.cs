using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Dialogs
{
    public class DialogAndAction
    {
        public DialogType DialogType { get; set; }
        public Action ActionType { get; set; }
        public string Message { get; set; }
    }
}