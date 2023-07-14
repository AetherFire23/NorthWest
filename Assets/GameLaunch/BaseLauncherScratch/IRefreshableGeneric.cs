using Cysharp.Threading.Tasks;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public interface IRefreshableGeneric<T> : IStateInteractor<T> where T : class, new()
    {
        public UniTask Refresh();
    }
}
