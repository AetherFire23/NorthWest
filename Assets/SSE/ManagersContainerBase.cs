using Assets.GameLaunch.BaseLauncherScratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HttpStuff
{
    public abstract class ManagersContainerBase<TState> : MonoBehaviour where TState : class, new()
    {
        private List<StateHolderBase<TState>> _stateHolders = new List<StateHolderBase<TState>>();
        public async UniTask InitializeAsync()
        {
            DiscoverAndRegisterManagersInScene();
        }

        public async UniTask ExecuteInitialData(TState state)
        {
            foreach (var item in _stateHolders)
            {
                await item.InitializeAsync(state);
            }
        }

        public async UniTask RefreshAllManagers(TState state)
        {
            foreach (var manager in _stateHolders)
            {
                await manager.RefreshAsync(state);
            }
        }

        private void DiscoverAndRegisterManagersInScene()
        {
            _stateHolders = UnityExtensions.FindAllSubclassesOf<StateHolderBase<TState>>();
        }
    }
}
