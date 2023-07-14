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
                    string parseableLine = await _disposables.StreamReader.ReadLineAsync().AsUniTask();

                    if (string.IsNullOrEmpty(parseableLine))
                    {
                        Debug.Log("No messages received");
                    }

                    Debug.Log($"Message Received {parseableLine}");

                    SSEClientData data = new SSEClientData(parseableLine);
                    await callback.Invoke(data);
                    await UniTask.Delay(250);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"<color=green> Expected Exception </color> SSE Disconnection : {ex.Message}");
                //  Debug.LogException(ex);
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
