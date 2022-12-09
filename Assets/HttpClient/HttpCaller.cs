using Assets.HttpClient.Shared_API_Models;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HttpClient
{
    public class HttpCaller
    {
        private readonly System.Net.Http.HttpClient _client;
        public HttpCaller(System.Net.Http.HttpClient client)
        {
            _client = client;
        }

        public async UniTask<ClientCallResult> PutRequest2(UriBuilder infos)
        {
            using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _client.PutAsync(infos.Path, stringContent).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"The following path : {infos.Path} did not lead to a successful request.");
            }
            response.EnsureSuccessStatusCode();
            string responseContent = response.Content.ReadAsStringAsync().Result;
            var clientCallResult = JsonConvert.DeserializeObject<ClientCallResult>(responseContent);
            return clientCallResult;
        }

        public async UniTask<T> GetRequest<T>(UriBuilder infos)
        {
            using HttpResponseMessage response = await _client.GetAsync(infos.Path).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($" The followng path <{infos.Path}> is invalid");
                response.EnsureSuccessStatusCode();
            }
            string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            T result = JsonConvert.DeserializeObject<T>(responseBody);
            return result;
        }

        public async UniTask<ClientCallResult> PostRequest(UriBuilder infos)
        {
            using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _client.PostAsync(infos.Path, stringContent).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"The following path : {infos.Path} did not lead to a successful request.");
            }
            response.EnsureSuccessStatusCode();
            string responseContent = response.Content.ReadAsStringAsync().Result;
            var clientCallResult = JsonConvert.DeserializeObject<ClientCallResult>(responseContent);
            return clientCallResult;
        }
    }
}
