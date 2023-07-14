using Assets.SSE;
using Cysharp.Threading.Tasks;
using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;
using UnityEngine;

namespace Assets.MainMenu.MenuSSE
{
    public class MainMenuSSERefresher : SSERefresherBase<MainMenuSSERefresher>
    {
        [EventMethodMapping(SSEType.Heartbeat)] // heart
        public async UniTask HeartBeat(SSEClientData data)
        {
            Debug.Log($"SSE HeartBeat successfully received");
        }
    }
}
