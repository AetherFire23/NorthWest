using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HttpStuff
{
    public class Datastore<TState> : MonoBehaviour where TState : class, new()
    {
        public TState State { get; set; }
        public async UniTask InitializeAsync(TState state)
        {
            State = state;
        }
    }
}
