using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameLaunch
{
    public interface IRefreshable
    {
        // GameState?
        public UniTask Refresh(GameState gameState);
    }
}
