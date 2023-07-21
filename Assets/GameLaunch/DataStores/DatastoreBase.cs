using Cysharp.Threading.Tasks;
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
