using Assets.SSE;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models.SSE;
using System;
using UnityEngine;
namespace Assets.HttpStuff
{
    public class SSEStream : IDisposable 
    {
        private readonly SSEStreamDisposables _disposables;
        bool _mustStopReceivingMessages = false;
        public SSEStream(SSEStreamDisposables disposables)
        {
            _disposables = disposables;
        }

        public async UniTask StartReceivingMessages(Func<SSEClientData, UniTask> callback)
        {
            try
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
                    await UniTask.Delay(200);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public async UniTask DummyNotAwaited()
        {
            while (!_mustStopReceivingMessages) 
            {
                Debug.Log("going and going");
                await UniTask.Delay(200);
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
