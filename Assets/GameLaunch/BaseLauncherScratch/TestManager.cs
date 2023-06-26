using Assets.HttpStuff;
using Assets.MainMenu.Launch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using UnityEngine;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public class TestManager : StateHolderBase<MainMenuState>
    {
        [SerializeField] MainMenuLauncher _stateHolder;
        [SerializeField] Calls _client;
        public override async UniTask InitializeAsync()
        {
            Debug.Log("I just initialized bitch");
        }

        public override async UniTask RefreshAsync()
        {
            Debug.Log($"I just Refreshed bitch{nameof(TestManager)} ");

            await UniTask.Delay(2);
        }
    }
}
