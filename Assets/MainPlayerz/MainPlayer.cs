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
        public Player playerModel;
        public RoomType CurrentRoomType = RoomType.Start;
        public Guid Id => playerModel.Id;
        public Guid GameId => playerModel.GameId;

        private readonly ClientCalls _clientCalls; 

        public MainPlayer(ClientCalls clientCalls )
        {
            Guid defaultGameGuid = new Guid("DE74B055-BA84-41A2-BAEA-4E380293E227");
            Guid defaultPlayer1Guid = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F");

            _clientCalls = clientCalls;
            var FredPlayerModel = new Player()
            {
                Id = defaultPlayer1Guid, // finir udpater les models de base
                GameId = defaultGameGuid,
                CurrentChatRoomId = new Guid("F76A4822-8D96-4073-E7F8-08DA70D59137"),
                Name = "Fred",
            };
            this.playerModel = FredPlayerModel;
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