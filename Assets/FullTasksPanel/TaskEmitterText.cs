using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FullTasksPanel
{
    public class TaskEmitterText : InstanceWrapper<BasicTextScript>
    {
        public TaskEmitterText(GameObject parent, string emitterType, string emitterName) : base("FullTaskPanel/TaskEmitterText", parent)
        {
            string emitterFormatted = $"{emitterType}:{emitterName}";
            this.AccessScript.TextMesh.text = emitterFormatted;
        }
    }
}
