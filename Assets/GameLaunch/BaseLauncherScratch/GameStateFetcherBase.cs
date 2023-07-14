using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public abstract class GameStateFetcherBase<T> : MonoBehaviour // maybe it should not be a MonoBehaviour honestly.  Just normal C Sharp class with constructor
        where T : class, new()
    {
        protected MainMenuCalls Client { get; set; }
        public async UniTask Initialize()
        {
            Client = UnityExtensions.FindUniqueMonoBehaviour<MainMenuCalls>();
        }

        protected DateTime? _lastTimeStamp { get; set; } = null;

        // mustb e implement because we cant know which parameters the api call will require
        public abstract UniTask<T> FetchFirstGameStateAsync(Guid playerId); // leaves timeStamp at null so I can initialize the whole thing instead of updating


        public abstract UniTask<T> FetchNextGameStateAsync(Guid playerId);

    }
}
