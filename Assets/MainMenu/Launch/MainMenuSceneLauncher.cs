using Assets.Constants;
using Assets.GameLaunch;
using Assets.HttpStuff;
using Assets.MainMenu.MenuSSE;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MainMenu.Launch
{
    [DefaultExecutionOrder(-10)]
    public class MainMenuSceneLauncher : SceneLauncherBase<MainMenuState, MainMenuCalls> // Will be MainMenuCalls
    {
        [SerializeField] private AuthenticationManager _authenticationManager;
        [SerializeField] private MainMenuSSERefresher _mainMenuSSERefresher;
        private SSEStream _mainMenuSSEStream;

        protected override async UniTask BeforeInitializingManagers()
        {
            await _authenticationManager.WaitUntilAuthenticated();
            ClientCalls.ConfigureAuthenticationHeaders(PersistenceModel.Instance.Token);

        }

        protected override async UniTask AfterInitializingManagers(MainMenuState state)
        {
            _mainMenuSSEStream = await ClientCalls.GetSSEStream(PersistenceReducer.PlayerId, PersistenceReducer.GameId);
            await _mainMenuSSERefresher.InitializeAsync(_mainMenuSSEStream);

            PersistenceReducer.PlayerId = state.UserDto.Id;
        }

        public async UniTask LoadGameSceneAsync()
        {
            var disposables = new List<IDisposable>
            {
                _mainMenuSSEStream,
            };
            await ChangeSceneAndDisposeResources(SceneNames.GameScene, disposables);
        }
        protected override async UniTask<MainMenuState> FetchInitialState()
            => await base.ClientCalls.GetMainMenuState(PersistenceReducer.UserId); // Setté pendant l'auth
    }
}