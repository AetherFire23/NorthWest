using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utils.ObjectExample
{
    public class SomeObject : GameObjectWrapper
    {
        public SomeObject(SomeObjectScript script) : base(script) // injected ! 
        {

        }
    }
}
