using Assets.GameLaunch.BaseLauncherScratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;

namespace Assets.SSE
{
    public class DummyStateManager : StateHolderBase<GameState>
    {
        public override async UniTask Initialize(GameState state)
        {
            
        }
    }
}
