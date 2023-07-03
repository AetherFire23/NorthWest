using Assets.GameLaunch;
using Assets.HttpStuff;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using Shared_Resources.Models.SSE;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MainMenu.Launch
{
    [DefaultExecutionOrder(-10)]
    public class MainMenuLauncher : SceneLauncherBase<MainMenuState, MainMenuCalls> // Will be MainMenuCalls
    {
        [SerializeField] private AuthenticationManager _authenticationManager;

        public override UniTask OnSSEDataReceived(SSEClientData data)
        {
            throw new System.NotImplementedException();
        }

        protected override async UniTask BeforeInitializingManagers()
        {
            Debug.Log("Initialze stuff here please. Like the login screen and so and so ");

            await _authenticationManager.WaitUntilAuthenticated();
            ClientCalls.ConfigureAuthenticationHeaders(PersistenceModel.Instance.Token);
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            // LoadScene, or check available games ? 
            // Yeah I mean I should make just a generic lobby I guess


            //


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
