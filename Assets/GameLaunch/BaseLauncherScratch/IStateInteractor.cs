using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public interface IStateInteractor<T> where T : class, new()
    {
        public T State { get; set; }
    }
}
