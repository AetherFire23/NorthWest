using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WebAPI.Models;

namespace Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem
{
    public class InviteButtonUGI : InstanceWrapper<InviteButtonInstanceScript>
    {
        public const string resourceName = "InviteButtonPrefab";
        public PrivateInvitation PrivateInvitation;

        public Player Player;
            // ToUserId
        // PlayerName
        // FromUserId
        public InviteButtonUGI(GameObject parent, PrivateInvitation invite ,string text) : base(resourceName, parent)
        {
            this.AccessScript.textComp.text = text;
            PrivateInvitation = invite;
        }
        public InviteButtonUGI(GameObject parent, Player player) : base(resourceName, parent)
        {
            Player = player;
            this.AccessScript.textComp.text = Player.Name;
        }
    }
}
