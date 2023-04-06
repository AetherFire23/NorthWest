using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using Assets.Raycasts.NewRaycasts;
using Shared_Resources.Models;

namespace Assets.HttpClient
{
    public class ChatCalls
    {
        private readonly HttpCaller _httpCaller;
        private readonly Guid _playerUID;
        public ChatCalls(System.Net.Http.HttpClient client, Guid playerId)
        {
            _httpCaller = new HttpCaller(client);
            _playerUID = playerId;
        }

        private const string _uriAddPrivateChatInvitation = "https://localhost:7060/Chat/InviteToRoom"; //require parameter
        public async UniTask<ClientCallResult> InviteToRoom(Guid targetPlayer, Guid targetRoomId)
        {
            var infos = new UriBuilder(_uriAddPrivateChatInvitation, ParameterOptions.Required)
                .WithParameter("fromId", _playerUID.ToString())
                .WithParameter("targetPlayer", targetPlayer.ToString())
                .WithParameter("targetRoomId", targetRoomId.ToString());
            var clientCallResult = _httpCaller.PutRequest2(infos).AsTask().Result;
            return clientCallResult;
        }

        private const string _uriSendInviteResponse = "https://localhost:7060/Chat/SendInviteResponse"; //require parameter
        public ClientCallResult SendInvitationResponse(Guid triggerId, bool isAccepted)
        {
            var infos = new UriBuilder(_uriSendInviteResponse, ParameterOptions.Required);
            infos.AddParameter("triggerId", triggerId.ToString());
            infos.AddParameter("isAccepted", isAccepted.ToString());
            var None = _httpCaller.PutRequest2(infos).AsTask().Result;
            return None;
        }

        private const string _uriCreateChatroom = "https://localhost:7060/Chat/CreateChatroom"; //require parameter
        public async UniTask<ClientCallResult> CreateChatroom(Guid newRoomGuid) // created dans le chat pour laffichage
        {
            var infos = new UriBuilder(_uriCreateChatroom, ParameterOptions.Required)
                .WithParameter("playerGuid", _playerUID.ToString())
                .WithParameter("newRoomGuid", newRoomGuid.ToString());
            var yeah = _httpCaller.PutRequest2(infos).AsTask().Result;
            return yeah;
        }

        private const string _uriLeavePrivateChatRoom = "https://localhost:7060/Chat/LeaveChatRoom"; //require parameter
        public async UniTask<ClientCallResult> LeaveChatRoom(Guid roomToLeave)
        {
            var infos = new UriBuilder(_uriLeavePrivateChatRoom, ParameterOptions.Required);
            infos.AddParameter("playerId", this._playerUID.ToString());
            infos.AddParameter("roomToLeave", roomToLeave.ToString());
            var yeah = _httpCaller.PutRequest2(infos).AsTask().Result;
            return yeah;
        }

        private const string _uriPutMessage = "https://localhost:7060/Chat/PutNewMessageToServer"; //require parameter 
        public ClientCallResult PutNewMessageToServer(Guid guid, Guid roomId, string newMessage) // pourrais changer pis Guid du player 
        {
            var infos = new UriBuilder(_uriPutMessage, ParameterOptions.Required);
            infos.AddParameter("guid", guid.ToString());
            infos.AddParameter("roomId", roomId.ToString());
            infos.AddParameter("receivedMessage", newMessage);
            var responseContent = _httpCaller.PutRequest2(infos).AsTask().Result;
            return responseContent;
        }
    }
}
