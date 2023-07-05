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
        /// </summary>
        public T State { get; set; }

        // make dictionary of values here 
        private static Dictionary<StateAction, Func<StateHolderBase<T>, UniTask>> StateActionsMap = new Dictionary<StateAction, Func<StateHolderBase<T>, UniTask>>()
        {
            { StateAction.Initialize, async holder =>  await holder.InitializeAsync()},
            { StateAction.Refresh, async holder =>  await holder.RefreshAsync()}
        };

        public async UniTask ExecuteActionAsync(StateAction actionType)
        {
            Func<StateHolderBase<T>, UniTask> action = StateActionsMap[actionType];
            await action(this);
        }
        
        public virtual async UniTask InitializeAsync() { }
        public virtual async UniTask RefreshAsync() { }
        public virtual async UniTask RefreshAsync(T state) { }
    }
}
