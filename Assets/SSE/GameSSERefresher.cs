using Assets.SSE;
using Cysharp.Threading.Tasks;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.HttpStuff
{
    public class GameSSERefresher : SSERefresherBase<GameSSERefresher> // should make generic
    {
        [SerializeField] private GameManagersContainer _managers;
        [SerializeField] private GameDataStore _gameDataStore;

        [EventMethodMapping(SSEEventType.DummyEvent)]
        public async UniTask Dummy1(SSEClientData data)
        {
            await _managers.RefreshAllManagers();
        }

        [EventMethodMapping(SSEEventType.RefreshPlayers)]
        public async UniTask Dummy2(SSEClientData data)
        {
            Debug.Log($"{nameof(SSEEventType.RefreshPlayers)}");

        }

        [EventMethodMapping(SSEEventType.RefreshItems)]
        public async UniTask Dummy3(SSEClientData data)
        {
            Debug.Log($"{nameof(SSEEventType.RefreshItems)}");

        }
    }
}