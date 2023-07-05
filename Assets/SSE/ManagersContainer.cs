using Assets.GameLaunch.BaseLauncherScratch;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HttpStuff
{
    public class ManagersContainer<TState> : MonoBehaviour where TState : class, new()
    {
        private List<StateHolderBase<TState>> _stateHolders = new List<StateHolderBase<TState>>();
        public async UniTask InitializeAsync()
        {
            DiscoverAndRegisterManagersInScene();
        }

        public async UniTask RefreshAllManagers(TState state)
        {

        }

        private void DiscoverAndRegisterManagersInScene()
        {
            _stateHolders = UnityExtensions.FindAllSubclassesOf<StateHolderBase<TState>>();
        }
    }
}
