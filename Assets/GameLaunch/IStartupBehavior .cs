using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameLaunch
{
    public interface IStartupBehavior
    {
        public UniTask Initialize(GameState gameState);
    }
}
