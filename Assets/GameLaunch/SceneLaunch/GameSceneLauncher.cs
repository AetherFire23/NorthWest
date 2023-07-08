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

        private SSEStream _sseStream;
        protected override async UniTask BeforeInitializingManagers()
        {
            PlayerInfo.UID = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");
            PlayerInfo.GameId = new Guid("DE74B055-BA84-41A2-BAEA-4E380293E227");
            MockToken();
        }

        protected override async UniTask AfterInitializingManagers()
        {
            var stream = await base.ClientCalls.GetSSEStream(PlayerInfo.UID, PlayerInfo.GameId);
            await _refresher.InitializeAsync(stream);
        }

        protected override async UniTask<GameState> FetchInitialState()
        {
            var state = await this.ClientCalls.GetGameState(new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F"), DateTime.UtcNow);
            return state;
        }

        private void MockToken()
        {
            PersistenceModel.Instance.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ1YmI1ZDU5LTMxNzEtNDA3My1iNzcwLWM3NmEyM2YyZTYyNSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJGcmVkIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUGVyZU5vZWwiLCJleHAiOjE3NzQ5MjQwNDMsImlzcyI6Imlzc3VlciIsImF1ZCI6ImF1ZGllbmNlIn0.jClMMjd7z7RSxYvEQa7ySMmJw6BeAQNx-UzqKNNiSRc";
        }

        private void OnApplicationQuit()
        {
            _sseStream.Dispose();
        }
    }
}
