using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.SSE
{
    public class SSERefresherBase<TREfresher> : MonoBehaviour where TREfresher : SSERefresherBase<TREfresher>
    {
        public Dictionary<SSEEventType, Func<SSEClientData, UniTask>> EnumDelegates { get; set; } = new();
        private SSEStream _sseStream;

        public async UniTask InitializeAsync(SSEStream sseStream)
        {
            _sseStream = sseStream;
            PopulateEnumDelegates();
            Debug.Log("Started receiving messages");
            _sseStream.StartReceivingMessagesCoroutine(OnDataReceived);
        }

        public async UniTask OnDataReceived(SSEClientData data) => await EnumDelegates[data.EventType].Invoke(data);

        [EventMethodMapping(SSEEventType.DummyEvent)]
        public async UniTask Dummy1(SSEClientData data)
        {
            Debug.Log($"{nameof(SSEEventType.DummyEvent)}");
        }

        [EventMethodMapping(SSEEventType.RefreshPlayers)]
        public async UniTask Dummy2(SSEClientData data)
        {
            Debug.Log($"{nameof(SSEEventType.RefreshPlayers)}");
        }

        [EventMethodMapping(SSEEventType.RefreshItems)]
        public async UniTask Dummy3(SSEClientData data)
        {
            Debug.Log($"{nameof(SSEEventType.RefreshItems)}");
        }

        public void PopulateEnumDelegates()
        {
            var refresher 
            var methods = typeof(GameSSERefresher).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(method => method.GetCustomAttributes(typeof(EventMethodMappingAttribute), true).Any())
                    .ToList();

            foreach (MethodInfo method in methods)
            {
                var customAttribute = method.GetCustomAttribute<EventMethodMappingAttribute>().EventType;
                var del = (Func<SSEClientData, UniTask>)method.CreateDelegate(typeof(Func<SSEClientData, UniTask>), this);
                this.EnumDelegates.Add(customAttribute, del);
            }
        }

        private void OnApplicationQuit()
        {
            _sseStream.Dispose();
            Debug.Log("Stopped Responding!");
        }
    }
}
