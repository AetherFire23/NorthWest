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
    public abstract class SSERefresherBase<TREfresher> : MonoBehaviour where TREfresher : SSERefresherBase<TREfresher>
    {
        private Dictionary<SSEType, Func<SSEClientData, UniTask>> _enumDelegates { get; set; } = new();
        private SSEStream _sseStream;

        public async UniTask InitializeAsync(SSEStream sseStream)
        {
            _sseStream = sseStream;
            PopulateEnumDelegates();
            Debug.Log("SSE : Started receiving messages");
            _sseStream.StartReceivingMessages(OnDataReceived);
        }

        public async UniTask OnDataReceived(SSEClientData data)
        {
            var deleg = _enumDelegates.GetValueOrDefault(data.EventType)
                ?? throw new ArgumentNullException($"Event data not foudn for {data.EventType}");

            await deleg.Invoke(data);
        }

        public void PopulateEnumDelegates()
        {
            var refresherSubclass = typeof(SSERefresherBase<TREfresher>).Assembly.GetTypes()
                .First(x => x.IsSubclassOf(typeof(SSERefresherBase<TREfresher>)));

            var methods = refresherSubclass.GetMethods()
                .Where(x => x.GetCustomAttributes(typeof(EventMethodMappingAttribute), false).Any());


            foreach (MethodInfo method in methods)
            {
                var customAttribute = method.GetCustomAttribute<EventMethodMappingAttribute>().EventType;
                var del = (Func<SSEClientData, UniTask>)method.CreateDelegate(typeof(Func<SSEClientData, UniTask>), this);
                this._enumDelegates.Add(customAttribute, del);
            }
        }

        private void OnApplicationQuit()
        {
            try
            {
                _sseStream.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            Debug.Log("SSE Stream cleaned up");
        }
    }
}
