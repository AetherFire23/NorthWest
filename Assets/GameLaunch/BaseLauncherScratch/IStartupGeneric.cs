using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public interface IStartupGeneric<T>  : IStateInteractor<T> where T : class, new() // switch to monobev%
    {
        public UniTask Initialize();
    }
}
