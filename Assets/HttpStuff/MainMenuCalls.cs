using Cysharp.Threading.Tasks;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using Shared_Resources.Models.SSE;
using System;

namespace Assets.HttpStuff
{
    public class MainMenuCalls : HttpCallerBase
    {
        private string GetFullEndpointMainMenu(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(MainMenuEndpoints), endpoint); // could be basefunc but whatevers
        private string GetFullEndpointUserController(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(UserEndpoints), endpoint); // could be basefunc but whatevers
        private string GetFullEndpointSSEStream(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(SSEEndpoints), endpoint); // could be basefunc but whatevers

        public override async UniTask InitializeEventStreamListening(Func<SSEClientData, UniTask> sseCallback)
        {
            var builder = new UriBuilder(GetFullEndpointSSEStream(SSEEndpoints.EventStream));
            base.ConfigureStream(builder, sseCallback);
        }

        public async UniTask<MainMenuState> GetMainMenuState()
        {
            // will have to implement a way to put the bearer shit into the request motherfucker for authorization

            string fullEndpoint = GetFullEndpointMainMenu(MainMenuEndpoints.State);
            var uriBuilder = new UriBuilder(fullEndpoint, ParameterOptions.None);

            MainMenuState mainMenuState = await base.HttpClient.GetRequest<MainMenuState>(uriBuilder)
                ?? throw new Exception("cant return null for mainMenuState");

            return mainMenuState;
        }

        /// <summary> Content is string of token </summary>
        public async UniTask<ClientCallResult> GetLoginToken(LoginRequest request)
        {
            string fullEndpoint = GetFullEndpointUserController(UserEndpoints.Login);
            var uriBuilder = new UriBuilder(fullEndpoint, ParameterOptions.BodyOnly, request);

            var result = await HttpClient.PostRequest(uriBuilder);
            return result;
        }

        public async UniTask<ClientCallResult> Register(RegisterRequest request)
        {
            string fullEndpoint = GetFullEndpointUserController(UserEndpoints.Register);
            var uriBuilder = new UriBuilder(fullEndpoint, ParameterOptions.BodyOnly, request);
            var result = await HttpClient.PostRequest(uriBuilder);
            return result;
        }
    }
}
