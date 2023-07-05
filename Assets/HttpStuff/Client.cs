using Assets.HttpStuff;
using Assets.SSE;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Shared_Resources.Models;
using Shared_Resources.Models.SSE;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using UnityEngine;

namespace Assets.GameLaunch
{
    /// <summary> manages low-level stuff about http calls </summary>
    public class Client : IDisposable
    {
        // https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines
        // should init client with a static method I think so that i can pass in the PooledConnectionLifetime 

        private HttpClient _client = new HttpClient()
        {
            Timeout = System.Threading.Timeout.InfiniteTimeSpan,

        };


        public void ConfigureAuthenticationHeaders(string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async UniTask<ClientCallResult> PutRequest2(UriBuilder infos)
        {
            using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json"); // stu pcq put et post require des body que je write le body cos y pourrait etre null mais whatever fuckoff
            using HttpResponseMessage response = await _client.PutAsync(infos.Path, stringContent).AsUniTask();

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"The following path : {infos.Path} did not lead to a successful request.");
            }
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync().AsUniTask();
            var clientCallResult = JsonConvert.DeserializeObject<ClientCallResult>(responseContent);
            return clientCallResult;
        }

        public async UniTask<T> GetRequest<T>(UriBuilder infos)
        {
            using HttpResponseMessage response = await _client.GetAsync(infos.Path).AsUniTask();
            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($" The followng path <{infos.Path}> is invalid");
                response.EnsureSuccessStatusCode();
            }
            string responseBody = await response.Content.ReadAsStringAsync().AsUniTask();
            T result = JsonConvert.DeserializeObject<T>(responseBody);
            return result;
        }

        public async UniTask<ClientCallResult> PostRequest(UriBuilder infos)
        {
            using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _client.PostAsync(infos.Path, stringContent).AsUniTask();

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"The following path : {infos.Path} did not lead to a successful request.");
            }
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync().AsUniTask();
            var clientCallResult = JsonConvert.DeserializeObject<ClientCallResult>(responseContent);
            return clientCallResult;
        }

        private bool _mustStopStream = false;
        public async UniTask StartStreamAsync3(string path, Func<SSEClientData, UniTask> callBack)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Accept", "text/event-stream");

            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync().AsUniTask();
            using var reader = new StreamReader(stream);

            while (!_mustStopStream)
            {
                try
                {
                    var line = await reader.ReadLineAsync().AsUniTask(); // READLINEASYNC YA PAS DE LINE OSTI

                    if (string.IsNullOrEmpty(line))
                    {
                        Debug.Log("No mesasge received");
                        continue;
                    }
                    else
                    {
                        var data = SSEClientData.ParseData(line);

                        await callBack.Invoke(data); // juste check

                        // something like ProcessData(SSEClientData)
                    }

                    Debug.Log(line);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        public async UniTask<SSEStream> GetSSEStream(string path)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Accept", "text/event-stream");

            using HttpResponseMessage response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            Stream stream = await response.Content.ReadAsStreamAsync().AsUniTask();
            var reader = new StreamReader(stream);

            SSEStreamDisposables disposables = new SSEStreamDisposables()
            {
                Request = request,
                Response = response,
                Stream = stream,
                StreamReader = reader,
            };
            SSEStream serverStream = new SSEStream(disposables);

            return serverStream;
        }

        //public virtual async UniTask ProcessSSEData(SSEClientData data) { }

        public void Dispose()
        {
            _client?.Dispose();
            Debug.Log("just cleaned up client");
        }
    }
}
