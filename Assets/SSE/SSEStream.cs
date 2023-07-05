using Assets.SSE;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models.SSE;
using System;
using UnityEngine;
namespace Assets.HttpStuff
{
    public class SSEStream : IDisposable // IHTTPClient one day to swap between 
    {
        private readonly SSEStreamDisposables _disposables;
        bool _mustStopReceivingMessages = false;
        public SSEStream(SSEStreamDisposables disposables)
        {
            _disposables = disposables;
        }

        public async UniTask StartReceivingMessages(Func<SSEClientData, UniTask> callback) // do something like add to a list outside
        {
            while (!_mustStopReceivingMessages)
            {
                string line = await _disposables.StreamReader.ReadLineAsync().AsUniTask();

                if (string.IsNullOrEmpty(line))
                {
                    Debug.Log("No messages received");
                }

                Debug.Log($"Message Received {line}");

                SSEClientData data = new SSEClientData(line);
                await callback(data);
            }
        }

        public void Dispose()
        {
            _mustStopReceivingMessages = true;
            _disposables.StreamReader.Dispose();
            _disposables.Stream.Dispose();
            _disposables.Request.Dispose();
            _disposables.Response.Dispose();
        }
    }
}
