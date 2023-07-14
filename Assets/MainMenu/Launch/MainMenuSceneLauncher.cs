using Assets.GameLaunch;
using Assets.HttpStuff;
using Assets.MainMenu.MenuSSE;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using Shared_Resources.Models.SSE;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MainMenu.Launch
{
    [DefaultExecutionOrder(-10)]
    public class MainMenuSceneLauncher : SceneLauncherBase<MainMenuState, MainMenuCalls> // Will be MainMenuCalls
    {
        [SerializeField] private AuthenticationManager _authenticationManager;
        [SerializeField] private MainMenuSSERefresher _mainMenuSSERefresher;
        protected override async UniTask BeforeInitializingManagers()
        {
            await _authenticationManager.WaitUntilAuthenticated();
            ClientCalls.ConfigureAuthenticationHeaders(PersistenceModel.Instance.Token);

        }

        protected override async UniTask AfterInitializingManagers()
        {
            var stream = await ClientCalls.GetSSEStream(PlayerInfo.Id, PlayerInfo.GameId);
            await _mainMenuSSERefresher.InitializeAsync(stream);
        }

        protected override async UniTask<MainMenuState> FetchInitialState()
            => await base.ClientCalls.GetMainMenuState(PlayerInfo.UserId); // Setté pendant l'auth
    }
}