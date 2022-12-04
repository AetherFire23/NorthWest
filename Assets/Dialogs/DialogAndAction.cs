using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.GameState_Management;

namespace Assets.Dialogs
{
    public class DialogAndAction
    {
        public TriggerNotificationDTO TriggerNotification { get; set; }
        public DialogType DialogType { get; set; }
        public Action ActionType { get; set; }
        public string Message { get; set; }
    }
}