using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Shared_Resources.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

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

        private bool _mustStopStream = false;


        public async UniTask<ClientCallResult> PutRequest2(UriBuilder infos)
        {
            using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json");
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


        public async UniTask StartStreamAsync1(UriBuilder infos) // using a plain stream
        {
            using var stream = await _client.GetStreamAsync(infos.Path).AsUniTask();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream && !_mustStopStream)
            {
                try
                {
                    Debug.Log("logging that shit");

                    string line1 = string.Empty;
                    await reader.ReadLineAsync().AsUniTask();

                    string line = await reader.ReadLineAsync().AsUniTask();
                    reader.Close();
                    //if (string.IsNullOrEmpty(line))
                    //{
                    //    Debug.Log("had to quit!");
                    //    continue;
                    //}

                    //if (line.StartsWith("data:"))
                    //{
                    //    var eventData = line.Substring(6); // data: + space ?
                    //    Debug.Log($"Your  data : {eventData}");
                    //}

                    //else if (line.StartsWith("event:"))
                    //{
                    //    var eventName = line.Substring(7);
                    //    Debug.Log("Received event name: " + eventName);
                    //}
                    await UniTask.Yield();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }


            }
        }

        public async UniTask StartStreamAsync2(UriBuilder infos) // this dont work
        {
            _client.DefaultRequestHeaders.Add("Accept", "text/event-stream");
            try
            {
                using HttpResponseMessage response = await _client.GetAsync(infos.Path, HttpCompletionOption.ResponseHeadersRead).AsUniTask();

                response.EnsureSuccessStatusCode();

                using Stream stream = await response.Content.ReadAsStreamAsync();
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!_mustStopStream)
                    {
                        var line = await reader.ReadLineAsync().ConfigureAwait(false);
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        if (line.StartsWith("data:"))
                        {
                            var eventData = line.Substring(6); // data: + space ?
                            Debug.Log($"Your sxy data : {eventData}");
                        }

                        else if (line.StartsWith("event:"))
                        {
                            var eventName = line.Substring(7);
                            Debug.Log("Received event name: " + eventName);
                        }
                    }
                }
            }
            catch (AggregateException ex)
            {
                Debug.LogException(ex);
            }

        }

        public async UniTask StartStreamAsync3(UriBuilder infos)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, infos.Path);
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!_mustStopStream)
            {
                try
                {
                    var line = await reader.ReadLineAsync(); // READLINEASYNC YA PAS DE LINE OSTI

                    if (line == null)
                        break;

                    Debug.Log(line);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }

            }
        }

        public void Dispose()
        {
            _mustStopStream = true;
            _client?.Dispose();
            Debug.Log("just cleaned up client");
        }
    }
}
