using Cysharp.Threading.Tasks;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using System;

namespace Assets.HttpStuff
{
    public class MainMenuCalls : HttpCallerBase2
    {
        private string GetFullEndpointMainMenu(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(MainMenuEndpoints), endpoint); // could be basefunc but whatevers
        private string GetFullEndpointUserController(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(UserEndpoints), endpoint); // could be basefunc but whatevers
        private string GetFullEndpointSSEStream(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(SSEEndpoints), endpoint); // could be basefunc but whatevers

        public async UniTask<MainMenuState> GetMainMenuState(Guid userId)
        {
            string fullEndpoint = GetFullEndpointMainMenu(MainMenuEndpoints.State);
            var uriBuilder = new UriBuilder(fullEndpoint, ParameterOptions.Required);
            uriBuilder.AddParameter("userId", userId.ToString());

            MainMenuState mainMenuState = await GetRequest<MainMenuState>(uriBuilder)
                ?? throw new Exception("cant return null for mainMenuState");

            return mainMenuState;
        }

        /// <summary> Content is string of token </summary>
        public async UniTask<ClientCallResult> GetLoginToken(LoginRequest request)
        {
            string fullEndpoint = GetFullEndpointUserController(UserEndpoints.Login);
            var uriBuilder = new UriBuilder(fullEndpoint, ParameterOptions.BodyOnly, request);

            var result = await PostRequest(uriBuilder);
            return result;
        }

        public async UniTask<ClientCallResult> Register(RegisterRequest request)
        {
            string fullEndpoint = GetFullEndpointUserController(UserEndpoints.Register);
            var uriBuilder = new UriBuilder(fullEndpoint, ParameterOptions.BodyOnly, request);
            var result = await PostRequest(uriBuilder);
            return result;
        }

        public async UniTask<SSEStream> GetSSEStream(Guid playerId, Guid gameId)
        {
            string path = SSEEndpoints.GetFullEndpointPath(SSEEndpoints.MainMenuStream);
            var builder = new UriBuilder(path, ParameterOptions.Required);
            builder.AddParameter("playerId", playerId.ToString());
            builder.AddParameter("gameId", gameId.ToString());
            SSEStream stream = await base.GetSSEStream(builder);
            return stream;
        }
    }
}
