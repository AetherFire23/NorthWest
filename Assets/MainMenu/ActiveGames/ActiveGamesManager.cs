using Assets.AssetLoading;
using Assets.EntityRefresh;
using Assets.GameLaunch.BaseLauncherScratch;
using Assets.HttpStuff;
using Assets.MainMenu.Launch;
using Assets.Scratch;
using Assets.Utils;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.MainMenu.LobbiesAndGames
{
    public class ActiveGamesManager : StateHolderBase<MainMenuState>
    {
        [SerializeField] private RectTransform _contentScrollView;
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private MainMenuSceneLauncher _mainMenuSceneLauncher;
        private ScratchEntityBaseRefresher<ActiveGameEntry, GameDto> _activeGameEntries;

        private MainMenuState _state;
        private List<GameDto> _gameDtos => _state.UserDto.ActiveGamesForUser;

        // could make snippet for initialize, refresh, refrshativeGames....
        public override async UniTask Initialize(MainMenuState state)
        {
            _state = state;
            _activeGameEntries = new(CreateNewActiveGame);
            await RefreshVisuals();
        }

        public override async UniTask Refresh(MainMenuState state)
        {
            _state = state;
            await RefreshVisuals();
        }

        private async UniTask RefreshVisuals()
        {
            await _activeGameEntries.RefreshEntites(_gameDtos);
        }

        private async UniTask<ActiveGameEntry> CreateNewActiveGame(GameDto dto)
        {
            ActiveGameEntry newEntry = await _prefabLoader.CreateInstanceOfAsync<ActiveGameEntry>(_contentScrollView.gameObject);
            Shared_Resources.Entities.Player playerInGame = _state.FindPlayerFromGameId(dto.Id);
            Func<UniTask> launchGameOnClick = async () => await OnJoinGameButtonClick(playerInGame.GameId);
            await newEntry.InitializeAsync(dto, playerInGame.Profession, launchGameOnClick);

            return newEntry;
        }

        // join game is not a call its just a scene change supported by wahts stored inside persistence
        private async UniTask OnJoinGameButtonClick(Guid gameId)
        {
            Debug.Log("Trying to join game");
            PersistenceReducer.UserId = _state.UserDto.Players.First(x => x.GameId == gameId).Id;
            PersistenceModel.Instance.GameId = gameId;

            await _mainMenuSceneLauncher.LoadGameSceneAsync();
        }
    }
}
