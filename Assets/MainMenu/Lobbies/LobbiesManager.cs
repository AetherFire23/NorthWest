using Assets.AssetLoading;
using Assets.EntityRefresh;
using Assets.GameLaunch.BaseLauncherScratch;
using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MainMenu.Lobbies
{
    public class LobbiesManager : StateHolderBase<MainMenuState>
    {
        [SerializeField] private MainMenuCalls _mainMenuCalls;
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private RectTransform _lobbiesScrollView;

        private ScratchEntityBaseRefresher<LobbyEntry, LobbyDto> _entityRefresher;
        private MainMenuState _state;
        public override async UniTask Initialize(MainMenuState state)
        {
            _entityRefresher = new(CreateLobbyEntry);
            await RefreshVisuals();
        }

        public override async UniTask Refresh(MainMenuState state)
        {
            await RefreshVisuals();
        }

        public async UniTask RefreshVisuals()
        {
            await RefreshLobbies();
        }

        public async UniTask RefreshLobbies()
        {
            var upToDateEntities = _state.UserDto.QueuedLobbies;
            await _entityRefresher.RefreshEntites(upToDateEntities);
        }

        public async UniTask<LobbyEntry> CreateLobbyEntry(LobbyDto lobbyDto)
        {
            var lobbyEntry = await _prefabLoader.CreateInstanceOfAsync<LobbyEntry>(_lobbiesScrollView.gameObject);
            await lobbyEntry.Initialize(lobbyDto);
            return lobbyEntry;
        }
    }
}
