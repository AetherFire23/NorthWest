using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FullTasksPanel
{
    public class TaskButton : InstanceWrapper<BasicButtonScript>
    {

        public TaskButton(GameObject parent, GameTaskAction gameTask) : base("FullTaskPanel/FullTaskButton", parent)
        {
            this.AccessScript.ButtonText.text = gameTask.taskName;
            this.AccessScript.ButtonComponent.AddMethod(gameTask.action);
        }
    }
}
