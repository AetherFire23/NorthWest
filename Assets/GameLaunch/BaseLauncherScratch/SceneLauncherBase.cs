using Assets.AssetLoading;
using Assets.GameLaunch.BaseLauncherScratch;
using Assets.HttpStuff;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
        private BaseManagerRefreshStopGuards _refreshStopGuards { get; set; } = new();


        private async UniTask Start()
        {
            if (_refreshStopGuards.MustPreventInitialization()) return;
            _refreshStopGuards.IsInitializing = true;
            await FindAndInitializeUtilityMonoBehaviours();

            await BeforeInitializingManagers();

            //must get State after Initializing managers since i need to authenticate
            // before fetching any state so I dont have the necessary UID 
            // when first launching
            var state = (await FetchInitialState()) ?? throw new System.Exception("State is null");

            await DataStore.InitializeAsync(state);
            await Managers.ExecuteInitialData();

            await AfterInitializingManagers();

            _refreshStopGuards.IsInitializing = false;
        }

        private async UniTask FindAndInitializeUtilityMonoBehaviours()
        {
            PrefabLoader = UnityExtensions.FindUniqueMonoBehaviour<PrefabLoader>(); // cant be serialized so...
            ClientCalls = UnityExtensions.FindUniqueMonoBehaviour<THTTPCaller>();
            Managers = UnityExtensions.FindUniqueMonoBehaviour<ManagersContainerBase<TState>>();
            DataStore = UnityExtensions.FindUniqueMonoBehaviour<DatastoreBase<TState>>();

            await PrefabLoader.InitializeAsync();
        
            await Managers.InitializeAsync();
        }

        protected abstract UniTask<TState> FetchInitialState();
        protected virtual async UniTask BeforeInitializingManagers() { }
        protected virtual async UniTask AfterInitializingManagers() { }
    }
}