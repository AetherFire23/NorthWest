using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
namespace Assets.GameLaunch
{
    public interface IStartupBehavior
    {

        public UniTask Initialize(GameState gameState);
    }
}
