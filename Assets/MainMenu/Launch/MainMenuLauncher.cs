using Assets.GameLaunch;
using Assets.HttpStuff;
using Assets.UtilsAssembly;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using UnityEngine;

namespace Assets.MainMenu.Launch
{
    [DefaultExecutionOrder(-10)]
    public class MainMenuLauncher : SceneLauncherBase<MainMenuState, Calls> // Will be MainMenuCalls
    {
        protected override async UniTask AfterInitializingManagers()
        {
            Debug.Log("Initialze stuff here please");
        }

        protected override async UniTask BeforeManagersRefresh()
        {
            Debug.Log("Pretend I just updated the player's position");
        }

        protected override async UniTask AfterManagersRefresh()
        {
            Debug.Log("Managers refreshed !");
            int tst = 2;
        }

        protected override async UniTask<MainMenuState> FetchState() => await base.ClientCalls.GetMainMenuState();
    }
}
