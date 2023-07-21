using Assets.HttpStuff;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using UnityEngine;

namespace Assets.GameLaunch.SceneLaunch
{
    [DefaultExecutionOrder(-99)]
    public class GameSceneLauncher : SceneLauncherBase<GameState, GameCalls>
    {
        [SerializeField] private GameSSERefresher _refresher;
        protected override async UniTask BeforeInitializingManagers()
        {
            var persistenceINfo = PersistenceModel.Instance;

            //PersistenceModel.Instance.PlayerId  = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");
            //PlayerInfo.GameId = new Guid("DE74B055-BA84-41A2-BAEA-4E380293E227");
            //MockToken();
        }

        protected override async UniTask AfterInitializingManagers(GameState state)
        {
            // SSE Last plz
            var stream = await base.ClientCalls.GetSSEStream(PersistenceReducer.PlayerId, PersistenceReducer.GameId);
            await _refresher.InitializeAsync(stream);

            Debug.Log("<color=green> Finished Initializing </color>");
        }

        protected override async UniTask<GameState> FetchInitialState()
        {
            var state = await this.ClientCalls.GetGameState(PersistenceReducer.PlayerId, null);
            return state;
        }
    }
}
