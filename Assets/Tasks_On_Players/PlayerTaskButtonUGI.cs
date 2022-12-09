using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Tasks_On_Players
{
    public class PlayerTaskButtonUGI : InstanceWrapper<PLayerTaskButtonScript>
    {
        public PlayerTaskButtonUGI(GameObject parent, string taskName) : base("PlayerTaskPrefab", parent)
        {
            this.AccessScript.Text.text = taskName;
        }
    }
}
