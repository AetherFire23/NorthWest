using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;
namespace Assets.ChatLog_Manager.Private_Rooms
{
    public class InviteResult // result = modele qui contient de linfo mais ne fait rien 
    {
        private readonly PrivateInvitation _privateInvite;
        public bool IsInvited = true;
        public string InviterName;

        public InviteResult(PrivateInvitation privateInvitation)
        {
            _privateInvite = privateInvitation; // declencher de la logiuqe dans un construicteur cest contre-intuitif 
            // se calculer soi-m[eme cest bizarre

            bool isEmptyGuid = _privateInvite.Id == new Guid(); // empty guid means no invitation - Ben ; retourner null serait mieux 
            if (isEmptyGuid)
            {
                IsInvited = false;
                return;
            }

            InviterName = _privateInvite.FromPlayerName;
        }

        public PrivateInvitation FulfillRequest(bool isAccepted)
        {
            _privateInvite.IsAccepted = isAccepted; // depends on paramater
            _privateInvite.RequestFulfilled = true; // always true when this is called
            return _privateInvite;
        }


    }
}
