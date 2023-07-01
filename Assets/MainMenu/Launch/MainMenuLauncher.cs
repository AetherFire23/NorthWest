using Assets.GameLaunch;
using Assets.HttpStuff;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using UnityEngine;

namespace Assets.MainMenu.Launch
{
    [DefaultExecutionOrder(-10)]
    public class MainMenuLauncher : SceneLauncherBase<MainMenuState, MainMenuCalls> // Will be MainMenuCalls
    {
        [SerializeField] private AuthenticationManager _authenticationManager;

        protected override async UniTask BeforeInitializingManagers()
        {
             await this.ClientCalls.SubscribeToServerSideEventsStream();



            //UniTask.Delay(50000);

            //  await _authenticationManager.WaitUntilAuthenticated();

            Debug.Log("Initialze stuff here please. Like the login screen and so and so ");
        }

        //protected override async UniTask AfterInitializingManagers()
        //{
        //    Debug.Log("Initialze stuff here please. Like the login screen and so and so ");
        //}

        //protected override async UniTask BeforeManagersRefresh()
        //{
        //    Debug.Log("Pretend I just updated the player's position");
        //}

        //protected override async UniTask AfterManagersRefresh()
        //{
        //    Debug.Log("AfterManagersRefresh");
        //    int tst = 2;
        //}

        protected override async UniTask<MainMenuState> FetchState() => await base.ClientCalls.GetMainMenuState();
    }
}
