﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.ChatLog_Manager;
using WebAPI.GameState_Management;
using WebAPI.Models;
using WebAPI.Models.DTOs;
using Assets.Room_Management;
using Assets.MainMenu;
using Assets.Big_Tick_Energy;
using Assets.Enums;

namespace Assets.GameState_Management
{
    public class GameStateManager : IInitializable
    {

        [Inject] TemporaryOptionsScript options;
        public List<PrivateChatRoomParticipant> ParticipantsInPlayerRooms => _currentGameState.PrivateChatRooms;
        public List<Message> NewMessages => _currentGameState.NewMessages ?? new();
        public List<Player> Players => _currentGameState.Players ?? new();
        public List<Player> OtherPlayers { get; set; }
        public PlayerDTO LocalPlayerDTO => _currentGameState.PlayerDTO ?? new();
        public RoomDTO Room => _currentGameState.Room ?? new();
        public List<Player> PlayersInRoom { get; set; }
        public List<TriggerNotificationDTO> Notifications => _currentGameState.TriggerNotifications;
        public Guid PlayerUID { get; set; } // inited through IInitializable

        private GameState _currentGameState; // could probably decouple the gamestate and its manager.
        private readonly GlobalTick _globalTick;
        private readonly ClientCalls _client;
        private bool _initialized = false;
        public GameStateManager(ClientCalls clientCalls, GlobalTick globalTick)
        {
            _globalTick = globalTick;
            _client = clientCalls;
        }

        public void Initialize() 
        {
            //var menuInfo = Resources.Load<MainMenuPersistence>("mainMenuPersistence");
            //Guid defaultPlayer1Guid = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F"); // fred
            //Guid defaultPlayer2Guid = new Guid("B3543B2E-CD81-479F-B99E-D11A8AAB37A0"); // ben
            // this.PlayerUID = defaultPlayer1Guid;


            this.PlayerUID = options.CurrentPlayerID;
            _client.Initialize(this.PlayerUID);

            // Load default guid if mainMenu was not loaded
            //this.PlayerUID = menuInfo.MainPlayerId == Guid.Empty
            //    ? defaultPlayer1Guid
            //    : menuInfo.MainPlayerId;

            if(String.IsNullOrEmpty(this.PlayerUID.ToString()))
            {
                throw new Exception("PlayerUID must be set");
            }

            _currentGameState = GetFirstGameState(); // forcing the program to receive gamestate since virtually everything depends on that. Is launched non-lazily from the zen installer.

            _globalTick.TimerTicked += OnTimerTick;
        }

        private GameState GetFirstGameState() // leaves timeStamp at null so I can initialize the whole thing instead of updating
        {
            DateTime? currentTimeStamp = null;
            var firstGameState = _client.GetGameState(this.PlayerUID, currentTimeStamp).AsTask().Result;
            UpdateOtherPlayers(firstGameState);
            return firstGameState;
        }

        private GameState GetNextGameState()
        {
            DateTime? currentTimeStamp = _currentGameState.TimeStamp;
            GameState newGameState = _client.GetGameState(this.PlayerUID, currentTimeStamp).AsTask().Result;

            if (true) ;
            UpdateOtherPlayers(newGameState);
            return newGameState;
       }

        private void UpdateOtherPlayers(GameState newGameState)
        {
            this.OtherPlayers = newGameState.Players.Where(x => x.Id != newGameState.PlayerDTO.Id).ToList();
            PlayersInRoom = OtherPlayers.Where(x => x.CurrentGameRoomId == newGameState.PlayerDTO.CurrentGameRoomId).ToList();
        }

        public List<TriggerNotificationDTO> GetNotifications(NotificationType notificationType)
        {
            var notifs = this.Notifications.Where(x => x.NotificationType == notificationType).ToList();
            return notifs;
        }

        private void OnTimerTick(object source, EventArgs e)
        {
            _globalTick.SubscribedMembers.Add(this.GetType().Name);
           _currentGameState = GetNextGameState();
        }

        
    }
}
