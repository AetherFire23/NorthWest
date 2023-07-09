using Assets.Utils;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public abstract class StateHolderBase<T> : MonoBehaviour where T : class, new() // switch to monobev%
    {
        /// <summary>
        /// State is initialized before InitializeAsync ans RefreshAsync()
        /// </summary>\
        /// 
        protected InitializationValues InitializationValues { get; set; } = new InitializationValues();

        public virtual async UniTask Initialize(T state) { }
        public virtual async UniTask Refresh(T state) { }
    }
}
