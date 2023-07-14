using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public abstract class StartupGenericBase<T> : MonoBehaviour where T : class, new()
    {
        public T State { get; set; }
        public abstract UniTask Initialize();
    }
}
