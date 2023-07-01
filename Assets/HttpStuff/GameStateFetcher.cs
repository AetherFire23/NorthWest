using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using UnityEngine;

namespace Assets.HttpStuff
{
    public class GameStateFetcher : MonoBehaviour
    {
        [SerializeField] private GameCalls _client;

        private DateTime? _lastTimeStamp = null;

        public async UniTask<GameState> FetchFirstGameStateAsync(Guid playerId) // leaves timeStamp at null so I can initialize the whole thing instead of updating
        {
            DateTime? currentTimeStamp = null;
            GameState firstGameState = await _client.GetGameState(playerId, currentTimeStamp);
            _lastTimeStamp = firstGameState.TimeStamp;
            return firstGameState;
        }

        public async UniTask<GameState> FetchNextGameStateAsync(Guid playerId)
        {
            GameState newGameState =  await _client.GetGameState(playerId, _lastTimeStamp);
            _lastTimeStamp = newGameState.TimeStamp;
            return newGameState;
        }
    }
}
