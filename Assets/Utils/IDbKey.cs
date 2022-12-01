using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utils
{
    /// <summary>
    /// Used to match identity between database models and unity gameobjects. 
    /// </summary>
    public interface IEntity
    {
        public Guid Id { get;}
    }
}
