using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.ChatLog_Manager.Private_Rooms.PlayerOptionsPanel
{
    public class PlayerOptionsPanelInstance : InstanceWrapper<PlayerOptionsPanelInstanceScript>
    {
        private const string resourceName = "PlayerOptionsPanelPrefab";
        public List<OptionEntryInstance> playerEntries = new List<OptionEntryInstance>();
        public PlayerOptionsPanelInstance(GameObject invitePanelCOntainer) : base(resourceName, invitePanelCOntainer)
        {


        }
    }
}
