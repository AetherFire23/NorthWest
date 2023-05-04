using Assets.AssetLoading;
using Assets.CHATLOG3;
using Assets.GameLaunch;
using Assets.GameState_Management;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogManager3
{
    public class LogManager3 : MonoBehaviour, IStartupBehavior, IRefreshable
    {
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private GameLogObjects _gameLogObjects;

        private List<LogTextObject> _logChatObjects = new();

        // vais pas implementer tout de suite les filtres specifiques parce que
        // je sais pas si jutilise le dropdown 
        private LogFilteringManager _filteringManager = new(); // set filter with filtering managers 

        private GameState _gameState;
        public async UniTask Initialize(GameState gameState)
        {
            _gameState = gameState;
            _filteringManager.AddRange(_gameState.Logs);
            await InitializeButtons();
            _filteringManager.SetPlayerFilter(PlayerInfo.UID);
            await RefreshLogsWithCurrentFilter();
        }

        public async UniTask Refresh(GameState gameState)
        {
            _gameState = gameState;
            _filteringManager.AddRange(_gameState.Logs);
            await RefreshLogsWithCurrentFilter();
        }

        public async UniTask RefreshLogsWithCurrentFilter()
        {
            var upToDateLogs = _filteringManager.GetUpToDateLogsWithCurrentFilter();
            await RefreshLogs(upToDateLogs);
        }

        public async UniTask InitializeButtons()
        {
            _gameLogObjects.OpenLogButton.AddMethod(() => _gameLogObjects.LogCanvas.enabled = !_gameLogObjects.LogCanvas.enabled);
        }

        public async UniTask RefreshLogs(List<Log> upToDateLogs) // might wanna filter par plus que des Guids ?
        {
            var appearedEntities = UnityExtensions.GetAppearedEntities(_logChatObjects, upToDateLogs);
            foreach (var appearedEntity in appearedEntities)
            {
                var chatObjects = await _prefabLoader.CreateInstanceOfAsync<LogTextObject>(_gameLogObjects.LogContentTransform.gameObject);
                await chatObjects.Initialize(appearedEntity);
                _logChatObjects.Add(chatObjects);
            }

            var disappearedGameObjects = UnityExtensions.GetDisappearedGameObjects(_logChatObjects, upToDateLogs);
            foreach (var disappeared in disappearedGameObjects)
            {
                _logChatObjects.Remove(disappeared);
                GameObject.Destroy(disappeared.gameObject);
            }
        }
    }
}
