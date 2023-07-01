using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    // must be custom class and the reason is that 
    // MonoBehaviour are injected into serializedFields like Calls because other monos do all kinds of calls all the time.
    public class StateFetcherBase<T> // maybe it should not be a MonoBehaviour honestly.  Just normal C Sharp class with constructor
        where T : class, new()
    {
        protected MainMenuCalls Client { get; set; }
        public StateFetcherBase(MainMenuCalls client)
        {
            Client = client;
            _lastTimeStamp = null;
        }

        protected DateTime? _lastTimeStamp { get; set; }

        // mustb e implement because we cant know which parameters the api call will require
        public virtual UniTask<T> FetchFirstGameStateAsync()
        {
            throw new NotImplementedException();
        }
        // leaves timeStamp at null so I can initialize the whole thing instead of updating


        public virtual UniTask<T> FetchNextGameStateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
