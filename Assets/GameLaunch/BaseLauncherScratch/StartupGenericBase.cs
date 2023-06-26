using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public abstract class StartupGenericBase<T> : MonoBehaviour where T : class, new()
    {
        public T State { get; set; }
        public abstract UniTask Initialize();
    }
}
