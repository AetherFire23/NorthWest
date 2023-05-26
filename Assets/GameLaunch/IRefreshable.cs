using Cysharp.Threading.Tasks;
using Shared_Resources.Models;

namespace Assets.GameLaunch
{
    public interface IRefreshable
    {
        // GameState?
        public UniTask Refresh(GameState gameState);
    }
}
