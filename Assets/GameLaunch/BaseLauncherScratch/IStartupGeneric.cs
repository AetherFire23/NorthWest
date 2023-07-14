using Cysharp.Threading.Tasks;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public interface IStartupGeneric<T> : IStateInteractor<T> where T : class, new() // switch to monobev%
    {
        public UniTask Initialize();
    }
}
