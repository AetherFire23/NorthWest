using Assets.SSE;
using Cysharp.Threading.Tasks;
using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.HttpStuff
{
    public class SSERefresher : MonoBehaviour
    {
        private SSEStream _sseStream;

        public Dictionary<SSEEventType, Func<SSEClientData, UniTask>> EnumDelegates { get; set; }

        public async UniTask InitializeAsync(SSEStream sseStream)
        {
            _sseStream = sseStream;
            _sseStream.StartReceivingMessages(OnDataReceived);
        }

        public async UniTask OnDataReceived(SSEClientData data) // this bad bpoy is awaited inside ssestream
        {
            await EnumDelegates[data.EventType].Invoke(data);
        }

        [EventMap(SSEEventType.DummyEvent)]
        public async UniTask FUckingDummy()
        {
            Debug.Log("Logging fucking dummy shit !");
        }

        public void PopulateEnumDelegates()
        {
            var methods = typeof(SSERefresher).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance)
                .Where(x => x.GetCustomAttribute<EventMapAttribute>() is not null)
                .ToList();

            foreach (MethodInfo method in methods)
            {
                var customAttribute = method.GetCustomAttribute<EventMapAttribute>().EventType;
                var del = (Func<SSEClientData, UniTask>)method.CreateDelegate(typeof(Func<SSEClientData, UniTask>), this);
                this.EnumDelegates.Add(customAttribute, del);
            }
        }

        private void OnApplicationQuit()
        {
            _sseStream.Dispose();
        }
    }
}