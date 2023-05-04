using Assets.CHATLOG3;
using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Shared_Resources.Entities;
using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FACTOR3
{
    public class Calls : MonoBehaviour
    {
        private const string _GetPlayers = "https://localhost:7060/TheCrew/GetPlayers";
        private const string _updatePositionByPlayerModel = "https://localhost:7060/TheCrew/UpdatePositionByPlayerModel"; // // requiresbodys
        private const string _uriInviteToChatRoom = "https://localhost:7060/TheCrew/InviteToChatRoom"; //require parameter
        private const string _uriGetPendingInvitations = "https://localhost:7060/TheCrew/GetPendingInvitations"; //require parameter
        private const string _uriGetPlayersCurrentGameChatRoom = "https://localhost:7060/TheCrew/GetPlayersCurrentGameChatRoom"; //require parameter
        private const string _uriAddPlayerRoomPair = "https://localhost:7060/TheCrew/AddPlayerRoomPair"; //require parameter
        private const string _uriGetGameState = "https://localhost:7060/TheCrew/GetGameState"; //require parameter
        private const string _uriTransferItem = "https://localhost:7060/TheCrew/TransferItem"; //require parameter
        private const string _uriChangeRoom = "https://localhost:7060/TheCrew/ChangeRoom"; //require parameter
        private const string _uriTryExeGameTask = "https://localhost:7060/TheCrew/TryExecuteGameTask"; //require body & param
        private const string _uriTest = "https://localhost:7060/TheCrew/dwdwdw"; //require body & param

        //ChatController
        // private const string _uriSendInvitationResponse = "https://localhost:7060/TheCrew/SendInvitationResponse"; //require parameter

        // private const string _uriAddPrivateChatInvitation = "https://localhost:7060/TheCrew/AddPrivateChatInvitation"; //require parameter



        private Client _httpClient = new Client();
        public async UniTask<GameState> GetGameState(Guid playerId, DateTime? lastTimeStamp)
        {
            GameState gameState = null;

            var infos = new UriBuilder(_uriGetGameState, ParameterOptions.Required);
            infos.AddParameter("playerId", playerId.ToString());
            string nullDateTimeAsString = lastTimeStamp.ToString() ?? ""; // DateTime is nullable
            infos.AddParameter("lastTimeStamp", nullDateTimeAsString);
            gameState = await _httpClient.GetRequest<GameState>(infos);


            if (gameState == null) { Debug.LogError("webapi not found"); }
            return gameState;
        }

        public async UniTask Blocking()
        {
            int x = 0;

            await UniTask.RunOnThreadPool(() =>
            {
                for (int i = 0; i < 9999999999; i++)
                {
                    x = i;
                }
            });
            Debug.Log("Finished counting");
        }

        public async UniTask<ClientCallResult> UpdatePosition(Player model)
        {
            var infos = new UriBuilder(_updatePositionByPlayerModel, ParameterOptions.BodyOnly, model); // p-t parameoptiers a none ?
            var result = await _httpClient.PutRequest2(infos);
            return result;
        }

        public async UniTask<ClientCallResult> TransferItemOwnerShip(Guid targetId, Guid itemId)
        {
            var infos = new UriBuilder(_uriTransferItem, ParameterOptions.Required);
            //infos.AddParameter("ownerId", ownerId.ToString());
            infos.AddParameter("targetId", targetId.ToString());
            infos.AddParameter("itemId", itemId.ToString());
            var result = await _httpClient.PutRequest2(infos);
            return result;
        }

        public UniTask<ClientCallResult> ChangeRoom(Guid playerId, string targetRoomName)
        {
            var infos = new UriBuilder(_uriChangeRoom, ParameterOptions.Required);
            infos.AddParameter("playerId", playerId.ToString());
            infos.AddParameter("targetRoomName", targetRoomName);
            var result = _httpClient.PutRequest2(infos);
            return result;
        }

        // Because all tasks pass through the same API call, creating different methods would be redundant
        //public ClientCallResult CookTask(Guid playerId, Dictionary<string, string> parameters)
        //{
        //    return TryExecuteGameTask(playerId, GameTaskType.Cook, parameters);
        //}

        public async UniTask<ClientCallResult> TryExecuteGameTask(Guid playerId, GameTaskCodes taskCode, Dictionary<string, string> parameters)
        {
            var infos = new UriBuilder(_uriTryExeGameTask, ParameterOptions.BodyAndParameter, parameters);
            infos.AddParameter("playerId", playerId.ToString());
            infos.AddParameter("taskCode", Convert.ToInt32(taskCode).ToString());
            var result = await _httpClient.PutRequest2(infos);
            return result;
        }

        private const string _uriPutMessage = "https://localhost:7060/Chat/PutNewMessageToServer"; //require parameter 
        public async UniTask<ClientCallResult> PutNewMessageToServer(Guid guid, Guid roomId, string newMessage) // pourrais changer pis Guid du player 
        {
            var infos = new UriBuilder(_uriPutMessage, ParameterOptions.Required);
            infos.AddParameter("guid", guid.ToString());
            infos.AddParameter("roomId", roomId.ToString());
            infos.AddParameter("receivedMessage", newMessage);
            var responseContent = await _httpClient.PutRequest2(infos);
            return responseContent;
        }

        private const string _uriAddPrivateChatInvitation = "https://localhost:7060/Chat/InviteToRoom"; //require parameter
        public async UniTask<ClientCallResult> InviteToRoom(RoomInvitationParameters parameters)
        {

            var infos = new UriBuilder(_uriAddPrivateChatInvitation, ParameterOptions.Required)
                .WithParameter("fromId", parameters.FromId.ToString())
                .WithParameter("targetPlayer", parameters.TargetPlayer.ToString())
                .WithParameter("targetRoomId", parameters.TargetRoomId.ToString());
            var clientCallResult = await _httpClient.PutRequest2(infos);
            return clientCallResult;
        }

        private const string _uriSendInviteResponse = "https://localhost:7060/Chat/SendInviteResponse"; //require parameter
        public ClientCallResult SendInvitationResponse(Guid triggerId, bool isAccepted)
        {
            var infos = new UriBuilder(_uriSendInviteResponse, ParameterOptions.Required);
            infos.AddParameter("triggerId", triggerId.ToString());
            infos.AddParameter("isAccepted", isAccepted.ToString());
            var None = _httpClient.PutRequest2(infos).AsTask().Result;
            return None;
        }

        private const string _uriCreateChatroom = "https://localhost:7060/Chat/CreateChatroom"; //require parameter
        public async UniTask<ClientCallResult> CreateChatroom(Guid playerUID, Guid newRoomGuid) // created dans le chat pour laffichage
        {
            var infos = new UriBuilder(_uriCreateChatroom, ParameterOptions.Required)
                .WithParameter("playerGuid", playerUID.ToString())
                .WithParameter("newRoomGuid", newRoomGuid.ToString());
            var yeah = await _httpClient.PutRequest2(infos);
            return yeah;
        }

        private const string _uriLeavePrivateChatRoom = "https://localhost:7060/Chat/LeaveChatRoom"; //require parameter
        public async UniTask<ClientCallResult> LeaveChatRoom(Guid playerUID, Guid roomToLeave)
        {
            var infos = new UriBuilder(_uriLeavePrivateChatRoom, ParameterOptions.Required);
            infos.AddParameter("playerId", playerUID.ToString());
            infos.AddParameter("roomToLeave", roomToLeave.ToString());
            var yeah = await _httpClient.PutRequest2(infos);
            return yeah;
        }


        private void OnApplicationQuit()
        {
            _httpClient.Dispose();
        }
    }
}
