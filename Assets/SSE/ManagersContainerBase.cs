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
        private DatastoreBase<TState> _datastore { get; set; }
        private List<StateHolderBase<TState>> _stateHolders = new List<StateHolderBase<TState>>();
        public async UniTask InitializeAsync()
        {
            _datastore = UnityExtensions.FindUniqueMonoBehaviour<DatastoreBase<TState>>();
            DiscoverAndRegisterManagersInScene();
        }

        public async UniTask ExecuteInitialData()
        {
            foreach (var item in _stateHolders)
            {
                await item.Initialize(_datastore.State);
            }
        }

        public async UniTask RefreshAllManagers()
        {
            foreach (var manager in _stateHolders)
            {
                await manager.Refresh(_datastore.State);
            }
        }

        private void DiscoverAndRegisterManagersInScene()
        {
            _stateHolders = UnityExtensions.FindAllSubclassesOf<StateHolderBase<TState>>();
        }
    }
}
