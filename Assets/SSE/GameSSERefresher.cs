using Assets.FullTasksPanel;
using Assets.INVENTORY3;
using Assets.LogManager3;
using Assets.SSE;
using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.HttpStuff
{
    public class GameSSERefresher : SSERefresherBase<GameSSERefresher> // should make generic
    {
        [SerializeField] private GameManagersContainer _managers;
        [SerializeField] private GameDataStore _gameDataStore;

        [EventMethodMapping(SSEType.Heartbeat)] // heart
        public async UniTask HeartBeat(SSEClientData data)
        {
            Debug.Log($"SSE HeartBeat successfully received");
        }

        [EventMethodMapping(SSEType.RefreshPlayers)]
        public async UniTask Dummy2(SSEClientData data)
        {
            // this will be server-side only, refresh players every 3 seconds
            Debug.Log($"{nameof(SSEType.RefreshPlayers)}");
        }

        [EventMethodMapping(SSEType.RefreshItems)] // Mettons player action -> 
        public async UniTask Dummy3(SSEClientData data)
        {
            var playerAndItems = data.Deserialize<PlayerAndRoomItems>();

            _gameDataStore.UpdateItems(playerAndItems);

            var managers = new List<Type>()
            {
                typeof(InventoryManager3),
                typeof(FullTasksManager),
            };

            await _managers.RefreshSpecificManagers(managers);
        }

        [EventMethodMapping(SSEType.AddLog)] // Mettons player action -> 
        public async UniTask Dummy4(SSEClientData data)
        {
            var log = data.Deserialize<Log>();

            _gameDataStore.State.Logs.Add(log);
            await _managers.RefreshSpecificManager<GameLogsManager3>();
        }

        [EventMethodMapping(SSEType.RefreshShipState)] // Mettons player action -> 
        public async UniTask Dummy6(SSEClientData data)
        {

        }

        [EventMethodMapping(SSEType.AddChatMessage)] // Mettons player action -> 
        public async UniTask Dummy5(SSEClientData data)
        {

        }
    }
}