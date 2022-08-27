using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem
{
    public class InviteButtonInstance : InstanceWrapper<InviteButtonInstanceScript>
    {
        public const string resourceName = "InviteButtonPrefab";
        public PrivateInvitation PrivateInvitation;

        public PlayerModel Player;
            // ToUserId
        // PlayerName
        // FromUserId
        public InviteButtonInstance(GameObject parent, PrivateInvitation invite ,string text) : base(resourceName, parent)
        {
            this.InstanceBehaviour.textComp.text = text;
            PrivateInvitation = invite;
        }
        public InviteButtonInstance(GameObject parent, PlayerModel player) : base(resourceName, parent)
        {
            Player = player;
            this.InstanceBehaviour.textComp.text = Player.Name;
        }
    }
}
