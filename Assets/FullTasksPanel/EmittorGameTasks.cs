using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FullTasksPanel
{
    public class EmittorGameTasks
    {
        public List<GameTaskAction> Actions { get; set; }
        public string EmitterName { get; set; }
        public string EmitterType { get; set; }

        public EmittorGameTasks(string emitterType, string emitterName)
        {
            this.Actions = new List<GameTaskAction>();
            EmitterName = emitterName;
            EmitterType = emitterType;
        }
    }
}
