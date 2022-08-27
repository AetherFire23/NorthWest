using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets
{
    public class MainPlayer : ITickable, IInitializable // je devrais utiliser le inheritance de UnityObject pour gerer linstance et le script
    {
        public PlayerModel playerModel;
        public RoomType CurrentRoomType = RoomType.Start;
        public Guid Id => playerModel.Id;
        public Guid GameId => playerModel.GameId;

        private readonly ClientCalls _clientCalls; 

        public MainPlayer(ClientCalls clientCalls )
        {
            _clientCalls = clientCalls;
            var FredPlayerModel = new PlayerModel()
            {
                Id = new Guid("F0415BB0-5F22-4E79-1D4C-08DA5F69A35F"),
                GameId = new Guid("F76A4822-8D96-4073-E7F8-08DA70D59137"),
                CurrentChatRoomId = new Guid("F76A4822-8D96-4073-E7F8-08DA70D59137"),
                Name = "Fred",
            };

            var benPlayerModel = new PlayerModel()
            {
                Id = new Guid("8B30559A-5237-4EE0-5769-08DA60F39FEF"),
                CurrentChatRoomId = new Guid("F76A4822-8D96-4073-E7F8-08DA70D59137"),
                Name = "ben",
                GameId = new Guid("F76A4822-8D96-4073-E7F8-08DA70D59137"),
            };

            this.playerModel = benPlayerModel;
        }

        public void Initialize()
        {
            InitializePlayerToGlobalChat();
        }

        public void Tick()
        {
           
        }

        public void InitializePlayerToGlobalChat()
        {
            var yeah = _clientCalls.UpdateCurrentRoomId(this.playerModel.Id, this.playerModel.CurrentChatRoomId).AsTask().Result;
        }
    }
}