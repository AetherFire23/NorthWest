using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models.SSE;
using System;
using UnityEngine;

namespace Assets.HttpStuff
{
    public abstract class HttpCallerBase : MonoBehaviour
    {
        protected DateTime? _timeStamp;
        protected Client HttpClient = new Client();





        public async virtual UniTask InitializeEventStreamListening(Func<SSEClientData, UniTask> sseCallback) { Debug.Log("event stream not configured"); }


        public void ConfigureAuthenticationHeaders(string token) => HttpClient.ConfigureAuthenticationHeaders(token);


        private void OnApplicationQuit()
        {
            HttpClient.Dispose();
        }
    }
}
