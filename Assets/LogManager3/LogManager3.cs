using Assets.AssetLoading;
using Assets.GameLaunch;
using Assets.GameState_Management;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LogManager3
{
    public class LogManager3 : MonoBehaviour, IStartupBehavior, IRefreshable
    {
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private GameLogObjects _gameLogObjects;

        [SerializeField] private TMP_Dropdown _playerDropDown;
        private DropDownManager<Player> _playerDropDownManager;

        [SerializeField] private TMP_Dropdown _roomsDropDown;
        private DropDownManager<RoomDTO> _roomsDropDownManager;

        private List<LogTextObject> _logChatObjects = new();

        // vais pas implementer tout de suite les filtres specifiques parce que
        // je sais pas si jutilise le dropdown 
        private LogFilteringManager _filteringManager = new(); // set filter with filtering managers 

        private GameState _gameState;
        public async UniTask Initialize(GameState gameState)
        {
            _gameState = gameState;
            _filteringManager.AddRange(_gameState.Logs);
            await InitializeDropDowns(gameState);
            await InitializeButtons();
            _filteringManager.SetPlayerFilter(PlayerInfo.UID);
            await RefreshLogsWithCurrentFilter();
        }

        private bool _isRefreshing = false;
        public async UniTask Refresh(GameState gameState)
        {

            _gameState = gameState;
            _filteringManager.AddRange(_gameState.Logs);
            await RefreshLogsWithCurrentFilter();
        }

        public async UniTask RefreshLogsWithCurrentFilter()
        {
            await WaitUntilRefreshEndsCoroutine();
            var upToDateLogs = _filteringManager.GetUpToDateLogsWithCurrentFilter();
            await RefreshLogs(upToDateLogs);
        }

        public async UniTask InitializeButtons()
        {
            _gameLogObjects.OpenLogButton.AddMethod(() => _gameLogObjects.LogCanvas.enabled = !_gameLogObjects.LogCanvas.enabled);
            Func<UniTask> viewAllAction = async () =>
            {
                _filteringManager.SetAllFilter();
                await RefreshLogsWithCurrentFilter();
            };
            _gameLogObjects.ViewAllButton.AddTaskFunc(viewAllAction);
        }

        // I Foudn that I dont actually have to destroy any log.
        // Since I can just disable the .enabled property of TextMeshProUGUI instead of deleting / creating.
        // So I can just Spawn every log and just diasble the incorrect ones instead of gameobject.destroy
        public async UniTask RefreshLogs(List<Log> upToDateLogs) 
        {
            var appearedEntities = UnityExtensions.GetAppearedEntities(_logChatObjects, upToDateLogs);
            foreach (var appearedEntity in appearedEntities)
            {
                var chatObjects = await _prefabLoader.CreateInstanceOfAsync<LogTextObject>(_gameLogObjects.LogContentTransform.gameObject);
                await chatObjects.Initialize(appearedEntity);
                _logChatObjects.Add(chatObjects);
            }

            var disappearedGameObjects = UnityExtensions.GetDisappearedGameObjects(_logChatObjects, upToDateLogs).ToList();
            foreach (var disappeared in disappearedGameObjects)
            {
                _logChatObjects.Remove(disappeared);
                GameObject.Destroy(disappeared.gameObject);
            }
        }

        public async UniTask InitializeDropDowns(GameState gameState)
        {
            // managers
            _playerDropDownManager = new DropDownManager<Player>(gameState.Players, _playerDropDown);

            _roomsDropDownManager = new DropDownManager<RoomDTO>(gameState.GetRoomsInAlphabeticalOrder(), _roomsDropDown);

            _playerDropDown.AddDropDownAction(async () =>
            {
                var selectedValue = _playerDropDownManager.GetSelectedValue();
                _filteringManager.SetPlayerFilter(selectedValue.Id);
                await RefreshLogsWithCurrentFilter();
            });

            _roomsDropDown.AddDropDownAction(async () =>
            {
                var selectedValue = _roomsDropDownManager.GetSelectedValue();
                _filteringManager.SetRoomFilter(selectedValue.Id);
                await RefreshLogsWithCurrentFilter();

            });
        }

        public async UniTask WaitUntilRefreshEndsCoroutine()
        {
            while (_isRefreshing)
            {
                Debug.Log("Waiting for refresh toe nd before updatingLogs");
                await UniTask.Yield();
            }
        }
    }
}
