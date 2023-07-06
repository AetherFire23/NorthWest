using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.HttpStuff
{
    public abstract class DatastoreBase<TState> : MonoBehaviour where TState : class, new()
    {
        public TState State { get; protected set; }
        public async UniTask InitializeAsync(TState state)
        {
            State = state;
        }
    }
}
