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
        private readonly MainPlayer _mainPlayer;

        public MessageService(ClientCalls clientCalls,
            MainPlayer mainPlayer)
        {
            _clientCalls = clientCalls;
            _mainPlayer = mainPlayer;
        }

        public void SendMessageToDatabase(string inputMessage, Guid roomId) 
        {
            if (_mainPlayer.playerModel == null) 
            {
                Debug.Log("You can not send message when the local player is not initialized.");
                return;
            }

            Guid playerId = _mainPlayer.playerModel.Id;
            string message = inputMessage;
            _clientCalls.PutNewMessageToServer(playerId, roomId, message);
        }
    }
}
