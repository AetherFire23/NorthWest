using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameLaunch.SceneLaunch
{
    public class GameSceneLauncher : SceneLauncherBase<GameState, GameCalls>
    {
        // faque je devrais surement faire un SSEData manager qui va pouvoir interagir avec le gameState
        public override UniTask OnSSEDataReceived(SSEClientData data)
        {
            // will have to make the manager do its thing 
            throw new NotImplementedException();
        }

        // ce sera pus le launcher qui va fetch le state. ca va être du SSE.
        protected override UniTask<GameState> FetchState()
        {
            throw new NotImplementedException();
        }
    }
}
