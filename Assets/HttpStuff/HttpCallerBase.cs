using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models.SSE;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

namespace Assets.HttpStuff
{
    public abstract class HttpCallerBase : MonoBehaviour 
    {
        protected DateTime? _timeStamp;
        protected Client HttpClient = new Client();





        public async virtual UniTask InitializeEventStreamListening(Func<SSEClientData, UniTask> sseCallback) { Debug.Log("event stream not configured"); }

        /// <summary> Dont await becuase stream is endless. Consider making coroutine version for more predictable behaviour.</summary>
        // peut-etre que je peux ladder dans le manager de la scene i dunno
        protected async virtual UniTask ConfigureStream(UriBuilder builder, Func<SSEClientData, UniTask> sseCallback)
        {
            HttpClient.StartStreamAsync3(builder.Path, sseCallback);
        }
        public void ConfigureAuthenticationHeaders(string token) => HttpClient.ConfigureAuthenticationHeaders(token);
        

        private void OnApplicationQuit()
        {
            HttpClient.Dispose();
        }
    }
}
