using Assets.GameLaunch.BaseLauncherScratch;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.HttpStuff
{
    public abstract class ManagersContainerBase<TState> : MonoBehaviour where TState : class, new()
    {
        private DatastoreBase<TState> _datastore { get; set; }
        private List<StateHolderBase<TState>> _stateHolders = new List<StateHolderBase<TState>>();
        private Dictionary<Type, StateHolderBase<TState>> _typeManagerMap = new();
        public async UniTask InitializeAsync()
        {
            _datastore = UnityExtensions.FindUniqueMonoBehaviour<DatastoreBase<TState>>();
            DiscoverAndRegisterManagersInScene();
            MapManagerTypes(_stateHolders);
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

        public async UniTask RefreshSpecificManager<T>() where T : StateHolderBase<TState>
        {
            var manager = _typeManagerMap[typeof(T)];
            await manager.Refresh(_datastore.State);
        }

        public async UniTask RefreshSpecificManagers(List<Type> types)
        {
            foreach (Type type in types)
            {
                var manager = _typeManagerMap[type];
                await manager.Refresh(_datastore.State);
            }
        }

        private void DiscoverAndRegisterManagersInScene()
        {
            _stateHolders = UnityExtensions.FindAllSubclassesOf<StateHolderBase<TState>>();
        }

        private void MapManagerTypes(List<StateHolderBase<TState>> managers)
        {
            foreach (StateHolderBase<TState> manager in managers)
            {
                _typeManagerMap.Add(manager.GetType(), manager);
            }
        }
    }
}
