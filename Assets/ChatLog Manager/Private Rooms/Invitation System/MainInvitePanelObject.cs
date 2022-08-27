using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem
{
    public class MainInvitePanelObject : GameObjectWrapper, ITickable, IInitializable
    {
        private MainInvitePanelObjectScript mainInvitePanelObjectScript;
        public MainInvitePanelObject(MainInvitePanelObjectScript script) : base(script) // injected ! 
        {
            mainInvitePanelObjectScript = script;
        }

        public void Initialize()
        {
        }

        public void Tick()
        {

        }
    }
}
