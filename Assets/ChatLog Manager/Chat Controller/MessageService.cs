using Assets.GameState_Management;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.ChatLog_Manager
{
    public class MessageService
    {
        private readonly ClientCalls _clientCalls;
        private readonly GameStateManager _gameStateManager;
        public MessageService(ClientCalls clientCalls, GameStateManager gameStateManager)
        {
            _clientCalls = clientCalls;
            _gameStateManager = gameStateManager;
        }

        public void SendMessageToDatabase(string inputMessage, Guid roomId) 
        {
            if (_gameStateManager.PlayerUID == null) 
            {
                Debug.Log("You can not send message when the local player is not initialized.");
                return;
            }

            Guid playerId = _gameStateManager.PlayerUID;
            string message = inputMessage;
            _clientCalls.PutNewMessageToServer(playerId, roomId, message);
        }
    }
}
