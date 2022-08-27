using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ChatLog_Manager.Private_Rooms.InvitePanelObject.PlayerOptionsPanel.OptionEntryInstance
{
    public abstract class OptionEntryInfo
    {
        public abstract string Name { get; set; }

        public abstract void EntryAction();
    }
}
