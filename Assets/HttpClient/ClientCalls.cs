using Assets.ChatLog_Manager;
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
using WebAPI.GameState_Management;
using WebAPI.Models;

public class ClientCalls
{
    private readonly HttpClient _client;

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
    private const string _uriPutMessage = "https://localhost:7060/TheCrew/PutNewMessageToServer"; //require parameter 
    private const string _uriSendInvitationResponse = "https://localhost:7060/TheCrew/SendInvitationResponse"; //require parameter


    public ClientCalls()
    {
        _client = new HttpClient();

    }

    ~ClientCalls()
    {
        _client.Dispose();
    }

    public void DestroyClient()
    {
        _client.CancelPendingRequests();
        _client.Dispose();
    }

    public async UniTask<GameState> GetGameState(Guid playerId, DateTime? lastTimeStamp)
    {
        try
        {
            var infos = new ControllerRequestInformation(_uriGetGameState, ParameterOptions.RequiresParameter);
            infos.AddParameter("playerId", playerId.ToString());
            string nullDateTimeAsString = lastTimeStamp.ToString() ?? ""; // DateTime is nullable
            infos.AddParameter("lastTimeStamp", nullDateTimeAsString);
            var gameState = GetRequest<GameState>(infos).AsTask().Result;
            return gameState;
        }
        catch
        {
            Debug.LogError("Web API not found, please exit game");
            Application.Quit();
        }
        return null;
    }

    public async UniTask<string> AddPlayerRoomPair(Guid playerId, Guid newRoomGuid)
    {
        var infos = new ControllerRequestInformation(_uriAddPlayerRoomPair, ParameterOptions.RequiresParameter);
        infos.AddParameter("playerGuid", playerId.ToString());
        infos.AddParameter("newRoomGuid", newRoomGuid.ToString());
        var yeah = PutRequest(infos).AsTask().Result;
        return yeah;
    }

    public ClientCallResult Test()
    {
        var info = new ControllerRequestInformation(_uriTest, ParameterOptions.None);
        return PutRequest2(info).AsTask().Result;
    }

    public async UniTask<List<Player>> GetPlayersCurrentGameChatRoom(Guid playerGuid)
    {
        var infos = new ControllerRequestInformation(_uriGetPlayersCurrentGameChatRoom, ParameterOptions.RequiresParameter);
        infos.AddParameter("playerGuid", playerGuid.ToString());
        List<Player> currentPlayersInChatRoom = GetRequest<List<Player>>(infos).AsTask().Result;
        return currentPlayersInChatRoom;
    }

    public async UniTask<PrivateInvitation> GetPendingInvitation(Guid playerGuid) // must return Guid.empty;
    {
        var infos = new ControllerRequestInformation(_uriGetPendingInvitations, ParameterOptions.RequiresParameter);
        infos.AddParameter("playerGuid", playerGuid.ToString());
        PrivateInvitation invitation = GetRequest<PrivateInvitation>(infos).AsTask().Result;
        return invitation;
    }

    public void SendInvitationResponse(PrivateInvitation invitation)
    {
        var infos = new ControllerRequestInformation(_uriSendInvitationResponse, ParameterOptions.RequiresBody, invitation);
        var None = PutRequest(infos).AsTask().Result;
    }

    public string InviteToChatRoom(PrivateInvitation privateInvitation)
    {
        var infos = new ControllerRequestInformation(_uriInviteToChatRoom, ParameterOptions.RequiresBody, privateInvitation);
        var response = PutRequest(infos).AsTask().Result; // c un probleme ! renvoie tjrs un string ! 
        return response;
        // va falloir je check c quoi qui renvoie un string 
    }

    public string PutNewMessageToServer(Guid guid, Guid roomId, string newMessage) // pourrais changer pis Guid du player 
    {
        var infos = new ControllerRequestInformation(_uriPutMessage, ParameterOptions.RequiresParameter);
        infos.AddParameter("guid", guid.ToString());
        infos.AddParameter("roomId", roomId.ToString());
        infos.AddParameter("receivedMessage", newMessage);
        string responseContent = PutRequest(infos).AsTask().Result;
        return responseContent;
    }

    public async UniTask<List<Player>> GetPlayers()
    {
        var infos = new ControllerRequestInformation(_GetPlayers);
        List<Player> players = await GetRequest<List<Player>>(infos);
        return players;
    }
    public async UniTask UpdatePosition(Player model)
    {
        var infos = new ControllerRequestInformation(_updatePositionByPlayerModel, ParameterOptions.RequiresBody, model); // p-t parameoptiers a none ?
        await PutRequest(infos);
    }

    public async UniTask TransferItemOwnerShip(Guid ownerId, Guid targetId, Guid itemId)
    {
        var infos = new ControllerRequestInformation(_uriTransferItem, ParameterOptions.RequiresParameter);
        infos.AddParameter("ownerId", ownerId.ToString());
        infos.AddParameter("targetId", targetId.ToString());
        infos.AddParameter("itemId", itemId.ToString());
        var result = PutRequest(infos).AsTask().Result;
    }

    public string ChangeRoom(Guid playerId, string targetRoomName)
    {
        var infos = new ControllerRequestInformation(_uriChangeRoom, ParameterOptions.RequiresParameter );
        infos.AddParameter("playerId", playerId.ToString() );
        infos.AddParameter("targetRoomName", targetRoomName);
        var result = PutRequest(infos).AsTask().Result;

        return result;
    }

    public string CookTask(Guid playerId, string stationName)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            {"stationName", stationName }
        };
        return TryExecuteGameTask(playerId, GameTaskType.Cook, parameters);

    }

    private string TryExecuteGameTask(Guid playerId, GameTaskType taskCode, Dictionary<string, string> parameters)
    {
        var infos = new ControllerRequestInformation(_uriTryExeGameTask, ParameterOptions.RequiresBodyAndParameter, parameters);
        infos.AddParameter("playerId", playerId.ToString());
        infos.AddParameter("taskCode", Convert.ToInt32(taskCode).ToString());
        var result = PutRequest(infos).AsTask().Result;
        return result;
    }

    private async UniTask<string> PutRequest(ControllerRequestInformation infos)
    {
        using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await _client.PutAsync(infos.Path, stringContent).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError($"The following path : {infos.Path} did not lead to a successful request.");
        }
        response.EnsureSuccessStatusCode();
        string responseContent = response.Content.ReadAsStringAsync().Result;
        return responseContent;
    }

    private async UniTask<ClientCallResult> PutRequest2(ControllerRequestInformation infos)
    {
        using var stringContent = new StringContent(infos.SerializedBody, Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await _client.PutAsync(infos.Path, stringContent).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError($"The following path : {infos.Path} did not lead to a successful request.");
        }
        response.EnsureSuccessStatusCode();
        string responseContent = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<ClientCallResult>(responseContent);
    }

    private async UniTask<T> GetRequest<T>(ControllerRequestInformation infos)
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
}