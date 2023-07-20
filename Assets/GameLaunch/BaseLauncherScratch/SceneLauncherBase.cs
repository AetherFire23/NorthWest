using Assets.AssetLoading;
using Assets.GameLaunch.BaseLauncherScratch;
using Assets.HttpStuff;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameLaunch
{
    // pourrais faire un SceneLauncherHTTPSTATE : SCeneLauncherBase
    public abstract class SceneLauncherBase<TState, THTTPCaller> : MonoBehaviour  // T is state
        where TState : class, new()
        where THTTPCaller : HttpCallerBase2
    {
        protected PersistenceModel Persistence => PersistenceModel.Instance;
        protected PrefabLoader PrefabLoader { get; set; }
        protected THTTPCaller ClientCalls { get; set; } // Could use different Calls for MainMenu
        protected ManagersContainerBase<TState> Managers { get; set; }
        private DatastoreBase<TState> DataStore { get; set; }

        protected SSEStream _sseStream;

        // because async, Start can run and run over itself over and over again so i need to block it.
        private bool _isInitializing = false;
        private async UniTask Start()
        {
            if (_isInitializing) return;
            _isInitializing = true;
            await FindAndInitializeUtilityBehaviours();

            await BeforeInitializingManagers();

            //must get State after Initializing managers since i need to authenticate
            // before fetching any state so I dont have the necessary UID 
            // when first launching
            TState state = (await FetchInitialState()) ?? throw new System.Exception("State is null");

            await DataStore.InitializeAsync(state);
            await Managers.ExecuteInitialData();

            await AfterInitializingManagers(state);

            _isInitializing = false;
        }

        private async UniTask FindAndInitializeUtilityBehaviours()
        {
            PrefabLoader = UnityExtensions.FindUniqueMonoBehaviour<PrefabLoader>(); // cant be serialized so...
            ClientCalls = UnityExtensions.FindUniqueMonoBehaviour<THTTPCaller>();
            Managers = UnityExtensions.FindUniqueMonoBehaviour<ManagersContainerBase<TState>>();
            DataStore = UnityExtensions.FindUniqueMonoBehaviour<DatastoreBase<TState>>();

            await PrefabLoader.InitializeAsync();
            await Managers.InitializeAsync();
        }

        protected virtual async UniTask ChangeSceneAndDisposeResources(string nextSceneName, List<IDisposable> disposables = null)
        {
            disposables ??= new List<IDisposable>();
            disposables.Add(ClientCalls);

            disposables.ForEach(x => x.Dispose());

            await SceneManager.LoadSceneAsync(nextSceneName).ToUniTask();
        }

        protected abstract UniTask<TState> FetchInitialState();
        protected virtual async UniTask BeforeInitializingManagers() { throw new NotImplementedException("implement before init managers"); }
        protected virtual async UniTask AfterInitializingManagers(TState state) { throw new NotImplementedException("implement after init managers"); }

        protected void OnApplicationQuit()
        {
            try
            {
                ClientCalls.Dispose();
                _sseStream.Dispose();
            }
            catch (Exception ex) 
            {
                Debug.Log($"Expected exception : {ex.Message}");
            }
        }
    }
}