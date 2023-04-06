using Assets.Utils;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Game_Logs
{
    // même objet que pour le messageLog juste que le constructor va changer pour
    // accomoder les privateLog et tout 
    public class LogMessageUGI : InstanceWrapper<TextObjectScript>, IEntity 
    {
        // faudra mettre la LogClass 

        public Log Log { get; set; }
        public Guid Id => this.Log.Id;
        public LogMessageUGI(GameObject parent, Log log) : base("ChatTextPrefab", parent)
        {
            this.Log = log;

            string text = string.Empty;
            string shortDate = log.Created.Value.ToShortTimeString();
            if (log.IsPublic)
            {
                
                text = $"({shortDate}) , {log.EventText}";
            }

            else
            {
                text = $"[Private] - ({shortDate}), {log.EventText}";
            }


            this.AccessScript.TextComponent.text = text;

        }
    }
}
