using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public interface IRefreshableGeneric<T> : IStateInteractor<T> where T : class, new()
    {
        public UniTask Refresh();
    }
}
