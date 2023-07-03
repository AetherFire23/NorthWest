using Assets.AssetLoading;
using Assets.GameLaunch.BaseLauncherScratch;
using Assets.HttpStuff;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameLaunch
{
    // pourrais faire un SceneLauncherHTTPSTATE : SCeneLauncherBase
    public abstract class SceneLauncherBase<TState, THTTPCaller> : MonoBehaviour  // T is state
        where TState : class, new()
        where THTTPCaller : HttpCallerBase
    {
        /// <summary> Call on Update() method and not on Refresh or Init </summary>
        public async UniTask ForceRefreshManagers()
        {
            if (_refreshStopGuards.IsInitializing) throw new Exception("Cant force refresh managers of none are inited");

            await WaitUntilRefreshEndsCoroutine(); // in case it is already refresh based on the timer
            await ExecuteOnAllManagers(StateAction.Refresh);
        }

        protected PersistenceModel Persistence => PersistenceModel.Instance;
        protected PrefabLoader PrefabLoader { get; set; }
        protected THTTPCaller ClientCalls { get; set; } // Could use different Calls for MainMenu

        /// <summary> Before the first http call</summary>
        protected virtual async UniTask BeforeInitializingManagers() { }
        protected virtual async UniTask AfterInitializingManagers() { }
        //protected virtual async UniTask BeforeManagersRefresh() { }
        //protected virtual async UniTask AfterManagersRefresh() { }
        protected abstract UniTask<TState> FetchState();
        private List<StateHolderBase<TState>> _stateHolders { get; set; } = new();
        private BaseManagerRefreshStopGuards _refreshStopGuards { get; set; } = new();

        private async UniTask Awake()
        {
            if (_refreshStopGuards.MustPreventInitialization()) return;
            _refreshStopGuards.IsInitializing = true;

            FindAndInitializeUtilityMonoBehaviours();
            DiscoverAndRegisterManagersInScene();

            await BeforeInitializingManagers();
            await ExecuteOnAllManagers(StateAction.Initialize);
            await AfterInitializingManagers();

            _refreshStopGuards.IsInitializing = false;
        }

        public virtual async UniTask OnSSEDataReceived(SSEClientData data) { }

        //private async UniTask Update()
        //{
        //    if (_refreshStopGuards.MustPreventRefreshing(Time.deltaTime)) return; // after this, is refrshing

        //    _refreshStopGuards.IsRefreshing = true;
        //    await BeforeManagersRefresh();
        //    await ExecuteOnAllManagers(StateAction.Refresh);
        //    await AfterManagersRefresh();

        //    _refreshStopGuards.IsRefreshing = false;
        //}

        private async UniTask ExecuteOnAllManagers(StateAction actionType) // should probaly do a whole other pattern if I had t o
        {
            TState state = await FetchState() ?? throw new Exception("State can't be null");
            foreach (var stateHolder in _stateHolders)
            {
                stateHolder.State = state;
                await stateHolder.ExecuteActionAsync(actionType);
            }
        }

        private async void FindAndInitializeUtilityMonoBehaviours()
        {
            PrefabLoader = UnityExtensions.FindUniqueMonoBehaviour<PrefabLoader>(); // cant be serialized so...
            ClientCalls = UnityExtensions.FindUniqueMonoBehaviour<THTTPCaller>();
            await PrefabLoader.InitializeAsync();
        }

        private async UniTask WaitUntilRefreshEndsCoroutine()
        {
            while (_refreshStopGuards.IsRefreshing)
            {
                await UniTask.Yield();
            }
        }

        private void DiscoverAndRegisterManagersInScene()
        {
            _stateHolders = UnityExtensions.FindAllSubclassesOf<StateHolderBase<TState>>();
        }
    }
}