////using Assets.ChatLog_Manager;
////using Assets.GameState_Management;
////using Assets.HttpClient;
////using Cysharp.Threading.Tasks;
////using Shared_Resources.Entities;
////using Shared_Resources.Models;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Net.Http;
////using System.Text;
////using System.Threading.Tasks;
////using UnityEngine;
////using Zenject;
////using Newtonsoft.Json;

////public class ClientCalls
////{
////    // The Crew Controller
////    private const string _GetPlayers = "https://localhost:7060/TheCrew/GetPlayers";
////    private const string _updatePositionByPlayerModel = "https://localhost:7060/TheCrew/UpdatePositionByPlayerModel"; // // requiresbodys
////    private const string _uriInviteToChatRoom = "https://localhost:7060/TheCrew/InviteToChatRoom"; //require parameter
////    private const string _uriGetPendingInvitations = "https://localhost:7060/TheCrew/GetPendingInvitations"; //require parameter
////    private const string _uriGetPlayersCurrentGameChatRoom = "https://localhost:7060/TheCrew/GetPlayersCurrentGameChatRoom"; //require parameter
////    private const string _uriAddPlayerRoomPair = "https://localhost:7060/TheCrew/AddPlayerRoomPair"; //require parameter
////    private const string _uriGetGameState = "https://localhost:7060/TheCrew/GetGameState"; //require parameter
////    private const string _uriTransferItem = "https://localhost:7060/TheCrew/TransferItem"; //require parameter
////    private const string _uriChangeRoom = "https://localhost:7060/TheCrew/ChangeRoom"; //require parameter
////    private const string _uriTryExeGameTask = "https://localhost:7060/TheCrew/TryExecuteGameTask"; //require body & param
////    private const string _uriTest = "https://localhost:7060/TheCrew/dwdwdw"; //require body & param

////    //ChatController
////    private const string _uriSendInvitationResponse = "https://localhost:7060/TheCrew/SendInvitationResponse"; //require parameter

////    private const string _uriAddPrivateChatInvitation = "https://localhost:7060/TheCrew/AddPrivateChatInvitation"; //require parameter

////    public ChatCalls Chat;

////    ~ClientCalls()
////    {
////        _httpClient.Dispose();
////    }

////    public void DestroyClient()
////    {
////        _httpClient.CancelPendingRequests();
////        _httpClient.Dispose();
////    }

////    public void Initialize(Guid playerId)
////    {
////        this.PlayerUID = playerId;
////        _httpClient = new HttpClient();
////        Chat = new ChatCalls(_httpClient, PlayerUID);
////    }

////    public async UniTask<GameState> GetGameState(Guid playerId, DateTime? lastTimeStamp)
////    {
////        try
////        {
////            var infos = new UriBuilder(_uriGetGameState, ParameterOptions.Required);
////            infos.AddParameter("playerId", playerId.ToString());
////            string nullDateTimeAsString = lastTimeStamp.ToString() ?? ""; // DateTime is nullable
////            infos.AddParameter("lastTimeStamp", nullDateTimeAsString);
////            GameState gameState = await GetRequest<GameState>(infos);
////            return gameState;
////        }

////        catch
////        {
////            Debug.LogError("Web API not found, please exit game");
////            Application.Quit();
////        }
////        return null;
////    }

////    public async UniTask<ClientCallResult> UpdatePosition(Player model)
////    {
////        var infos = new UriBuilder(_updatePositionByPlayerModel, ParameterOptions.BodyOnly, model); // p-t parameoptiers a none ?
////        var result = await PutRequest2(infos);
////        return result;
////    }

////    public async UniTask<ClientCallResult> TransferItemOwnerShip(Guid ownerId, Guid targetId, Guid itemId)
////    {
////        var infos = new UriBuilder(_uriTransferItem, ParameterOptions.Required);
////        infos.AddParameter("ownerId", ownerId.ToString());
////        infos.AddParameter("targetId", targetId.ToString());
////        infos.AddParameter("itemId", itemId.ToString());
////        var result = PutRequest2(infos).AsTask().Result;
////        return result;
////    }

////    public ClientCallResult ChangeRoom(Guid playerId, string targetRoomName)
////    {
////        var infos = new UriBuilder(_uriChangeRoom, ParameterOptions.Required);
////        infos.AddParameter("playerId", playerId.ToString());
////        infos.AddParameter("targetRoomName", targetRoomName);
////        var result = PutRequest2(infos).AsTask().Result;
////        return result;
////    }

////    // Because all tasks pass through the same API call, creating different methods would be redundant
////    public ClientCallResult CookTask(Guid playerId, string stationName)
////    {
////        Dictionary<string, string> parameters = new Dictionary<string, string>()
////        {
////            {"stationName", stationName } // should create constants in DLL, of 
////        };
////        return TryExecuteGameTask(playerId, GameTaskType.Cook, parameters);
////    }

////    private ClientCallResult TryExecuteGameTask(Guid playerId, GameTaskType taskCode, Dictionary<string, string> parameters)
////    {
////        var infos = new UriBuilder(_uriTryExeGameTask, ParameterOptions.BodyAndParameter, parameters);
////        infos.AddParameter("playerId", playerId.ToString());
////        infos.AddParameter("taskCode", Convert.ToInt32(taskCode).ToString());
////        var result = PutRequest2(infos).AsTask().Result;
////        return result;
////    }

////    private async UniTask<ClientCallResult> PutRequest2(UriBuilder infos)
////    {
////        using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json");
////        using HttpResponseMessage response = await _httpClient.PutAsync(infos.Path, stringContent).ConfigureAwait(false);

////        if (!response.IsSuccessStatusCode)
////        {
////            Debug.LogError($"The following path : {infos.Path} did not lead to a successful request.");
////        }
////        response.EnsureSuccessStatusCode();
////        string responseContent = response.Content.ReadAsStringAsync().Result;
////        var clientCallResult = JsonConvert.DeserializeObject<ClientCallResult>(responseContent);
////        return clientCallResult;
////    }

////    private async UniTask<T> GetRequest<T>(UriBuilder infos)
////    {
////        using HttpResponseMessage response = await _httpClient.GetAsync(infos.Path).AsUniTask();
////        if (!response.IsSuccessStatusCode)
////        {
////            Debug.LogError($" The followng path <{infos.Path}> is invalid");
////            response.EnsureSuccessStatusCode();
////        }
////        string responseBody = await response.Content.ReadAsStringAsync().AsUniTask();
////        T result = JsonConvert.DeserializeObject<T>(responseBody);
////        return result;
////    }

////    private async UniTask<ClientCallResult> PostRequest(UriBuilder infos)
////    {
////        using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json");
////        using HttpResponseMessage response = await _httpClient.PostAsync(infos.Path, stringContent).ConfigureAwait(false);

////        if (!response.IsSuccessStatusCode)
////        {
////            Debug.LogError($"The following path : {infos.Path} did not lead to a successful request.");
////        }
////        response.EnsureSuccessStatusCode();
////        string responseContent = response.Content.ReadAsStringAsync().Result;
////        var clientCallResult = JsonConvert.DeserializeObject<ClientCallResult>(responseContent);
////        return clientCallResult;
////    }
////}