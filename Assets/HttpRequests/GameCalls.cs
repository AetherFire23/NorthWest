﻿using Assets.CHATLOG3;
using Cysharp.Threading.Tasks;
using Shared_Resources.Constants.Endpoints;
using Shared_Resources.Constants.Mapper;
using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using System;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Assets.HttpStuff
{
    public class GameCalls : HttpCallerBase2
    {
        private const string _GetPlayers = "https://localhost:7060/TheCrew/GetPlayers";
        private const string _updatePositionByPlayerModel = "https://localhost:7060/TheCrew/UpdatePositionById";
        private const string _uriInviteToChatRoom = "https://localhost:7060/TheCrew/InviteToChatRoom";
        private const string _uriGetPendingInvitations = "https://localhost:7060/TheCrew/GetPendingInvitations";
        private const string _uriGetPlayersCurrentGameChatRoom = "https://localhost:7060/TheCrew/GetPlayersCurrentGameChatRoom";
        private const string _uriAddPlayerRoomPair = "https://localhost:7060/TheCrew/AddPlayerRoomPair";
        private const string _uriGetGameState = "https://localhost:7060/TheCrew/GetGameState";
        private const string _uriTransferItem = "https://localhost:7060/TheCrew/TransferItem";
        private const string _uriChangeRoom = "https://localhost:7060/TheCrew/ChangeRoom";
        private const string _uriTryExeGameTask = "https://localhost:7060/TheCrew/TryExecuteGameTask";
        private const string _uriTest = "https://localhost:7060/TheCrew/dwdwdw";

        //ChatController
        // private const string _uriSendInvitationResponse = "https://localhost:7060/TheCrew/SendInvitationResponse"; //require parameter
        // private const string _uriAddPrivateChatInvitation = "https://localhost:7060/TheCrew/AddPrivateChatInvitation"; //require parameter


        public string GetSSEEndpoint(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(SSEEndpoints), endpoint);
        //public string GetChatEndpoint(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(Chat), endpoint);
        public string GetGameEndpoints(string endpoint) => EndpointPathsMapper.GetFullEndpoint(typeof(GameEndpoints), endpoint);

        public async UniTask<GameState> GetGameState(Guid playerId, DateTime? lastTimeStamp)
        {
            var infos = new UriBuilder(GameEndpoints.GetFullPath(GameEndpoints.GameState), ParameterOptions.Required);
            infos.AddParameter("playerId", playerId.ToString());
            string nullDateTimeAsString = lastTimeStamp.ToString() ?? ""; // DateTime is nullable
            infos.AddParameter("lastTimeStamp", nullDateTimeAsString);
            ClientCallResult gameStateCallResult = await GetRequest(infos);
            GameState gamestate = gameStateCallResult.DeserializeContent<GameState>();

            if (gamestate == null) { Debug.LogError("webapi not found"); }
            return gamestate;
        }

        public async UniTask<ClientCallResult> UpdatePosition(Guid playerId, float x, float y)
        {
            var infos = new UriBuilder(GameEndpoints.GetFullPath(GameEndpoints.UpdatePlayerPosition), ParameterOptions.Required); // p-t parameoptiers a none ?
            infos.AddParameter("playerId", playerId.ToString());
            infos.AddParameter("x", x.ToString());
            infos.AddParameter("y", y.ToString());
            var result = await PutRequest2(infos);
            return result;
        }

        public async UniTask<ClientCallResult> TransferItemOwnerShip(Guid targetId, Guid ownerId, Guid itemId, Guid gameId)
        {
            var infos = new UriBuilder(GameEndpoints.GetFullPath(GameEndpoints.TransferItem), ParameterOptions.Required);
            //infos.AddParameter("ownerId", ownerId.ToString());
            infos.AddParameter("targetId", targetId.ToString());
            infos.AddParameter("ownerId", ownerId.ToString());
            infos.AddParameter("itemId", itemId.ToString());
            infos.AddParameter("gameId", gameId.ToString());
            var result = await base.PutRequest2(infos);
            return result;
        }

        public UniTask<ClientCallResult> ChangeRoom(Guid playerId, string targetRoomName)
        {
            var infos = new UriBuilder(GameEndpoints.GetFullPath(GameEndpoints.ChangeRoom), ParameterOptions.Required);
            infos.AddParameter("playerId", playerId.ToString());
            infos.AddParameter("targetRoomName", targetRoomName);
            var result = base.PutRequest2(infos);
            return result;
        }

        public async UniTask<ClientCallResult> TryExecuteGameTask(Guid playerId, GameTaskCodes taskCode, TaskParameters parameters)
        {
            var infos = new UriBuilder(GameEndpoints.GetFullPath(GameEndpoints.ExecuteGameTask), ParameterOptions.BodyAndParameter, parameters);
            infos.AddParameter("playerId", playerId.ToString());
            infos.AddParameter("taskCode", Convert.ToInt32(taskCode).ToString());
            var result = await base.PutRequest2(infos);
            return result;
        }

        // chat messages


        private const string _uriPutMessage = "https://localhost:7060/Chat/PutNewMessageToServer"; //require parameter 
        public async UniTask<ClientCallResult> PutNewMessageToServer(Guid guid, Guid roomId, string newMessage) // pourrais changer pis Guid du player 
        {
            var infos = new UriBuilder(_uriPutMessage, ParameterOptions.Required);
            infos.AddParameter("guid", guid.ToString());
            infos.AddParameter("roomId", roomId.ToString());
            infos.AddParameter("receivedMessage", newMessage);
            var responseContent = await base.PutRequest2(infos);
            return responseContent;
        }

        private const string _uriAddPrivateChatInvitation = "https://localhost:7060/Chat/InviteToRoom"; //require parameter
        public async UniTask<ClientCallResult> InviteToRoom(RoomInvitationParameters parameters)
        {

            var infos = new UriBuilder(_uriAddPrivateChatInvitation, ParameterOptions.Required)
                .WithParameter("fromId", parameters.FromId.ToString())
                .WithParameter("targetPlayer", parameters.TargetPlayer.ToString())
                .WithParameter("targetRoomId", parameters.TargetRoomId.ToString());
            var clientCallResult = await base.PutRequest2(infos);
            return clientCallResult;
        }

        private const string _uriSendInviteResponse = "https://localhost:7060/Chat/SendInviteResponse"; //require parameter
        public ClientCallResult SendInvitationResponse(Guid triggerId, bool isAccepted)
        {
            var infos = new UriBuilder(_uriSendInviteResponse, ParameterOptions.Required);
            infos.AddParameter("triggerId", triggerId.ToString());
            infos.AddParameter("isAccepted", isAccepted.ToString());
            var None = base.PutRequest2(infos).AsTask().Result;
            return None;
        }

        private const string _uriCreateChatroom = "https://localhost:7060/Chat/CreateChatroom"; //require parameter
        public async UniTask<ClientCallResult> CreateChatroom(Guid playerUID, Guid newRoomGuid) // created dans le chat pour laffichage
        {
            var infos = new UriBuilder(_uriCreateChatroom, ParameterOptions.Required)
                .WithParameter("playerGuid", playerUID.ToString())
                .WithParameter("newRoomGuid", newRoomGuid.ToString());
            var yeah = await base.PutRequest2(infos);
            return yeah;
        }

        private const string _uriLeavePrivateChatRoom = "https://localhost:7060/Chat/LeaveChatRoom"; //require parameter
        public async UniTask<ClientCallResult> LeaveChatRoom(Guid playerUID, Guid roomToLeave)
        {
            var infos = new UriBuilder(_uriLeavePrivateChatRoom, ParameterOptions.Required);
            infos.AddParameter("playerId", playerUID.ToString());
            infos.AddParameter("roomToLeave", roomToLeave.ToString());
            var yeah = await base.PutRequest2(infos);
            return yeah;
        }

        public async UniTask<SSEStream> GetSSEStream(Guid playerId, Guid gameId)
        {
            string path = this.GetSSEEndpoint(SSEEndpoints.EventStream);
            var builder = new UriBuilder(path, ParameterOptions.Required);
            builder.AddParameter("playerId", playerId.ToString());
            builder.AddParameter("gameId", gameId.ToString());
            SSEStream stream = await base.GetSSEStream(builder);
            return stream;
        }
    }
}
