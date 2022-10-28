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

namespace Assets.GameState_Management
{
    public class GameStateManager : ITickable, IInitializable
    {
        public List<PrivateChatRoomParticipant> ParticipantsInPlayerRooms => _currentGameState.PrivateChatRooms;
        public List<Message> NewMessages => _currentGameState.NewMessages ?? new();
        public List<Player> Players => _currentGameState.Players ?? new();
        public List<Player> OtherPlayers { get; set; }
        public PlayerDTO LocalPlayerDTO => _currentGameState.PlayerDTO ?? new();
        public List<PrivateInvitation> PrivateInvitations => _currentGameState.Invitations ?? new();
        public RoomDTO Room => _currentGameState.Room ?? new();
        public bool HasTicked => _tickTimer.HasTicked;
        public List<Player> PlayersInRoom { get; set; }
        public Guid PlayerUID { get; set; } // inited through IInitializable

        private GameState _currentGameState; // could probably decouple the gamestate and its manager.

        private readonly SimpleTimer _tickTimer = new SimpleTimer(3f);
        private readonly GlobalTick _globalTick;
        private readonly ClientCalls _clientCalls;

        public GameStateManager(ClientCalls clientCalls, GlobalTick globalTick)
        {
            _globalTick = globalTick;
            _clientCalls = clientCalls;
        }

        public void Initialize() // should verify if I even need a reference to UnityEngine.
        {
            var menuInfo = Resources.Load<MainMenuPersistence>("mainMenuPersistence");

            // Load default guid if mainMenu was not loaded
            this.PlayerUID = menuInfo.MainPlayerId == Guid.Empty
                ? new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F")
                : menuInfo.MainPlayerId;

            _currentGameState = GetFirstGameState(); // forcing the program to receive gamestate since virtually everything depends on that. Is launched non-lazily from the zen installer.

            _globalTick.TimerTicked += OnTimerTick;
        }

        public void Tick() // seems not needed anymore
        {

            //// UpdatePeriodicGameState ?

            //if (_tickTimer.HasTicked) // event for everytime it ticks is needed
            //{
            //    Debug.Log("The GameState timer has ticked");
            //    _tickTimer.Reset();
            //    _currentGameState = GetNextGameState();
            //}
            //_tickTimer.AddTime(Time.deltaTime);
        }

        private GameState GetFirstGameState() // leaves timeStamp at null so I can initialize the whole thing instead of updating
        {
            DateTime? currentTimeStamp = null;
            var firstGameState = _clientCalls.GetGameState(this.PlayerUID, currentTimeStamp).AsTask().Result;
            UpdateOtherPlayers(firstGameState);
            return firstGameState;
        }

        private GameState GetNextGameState()
        {
            DateTime? currentTimeStamp = _currentGameState.TimeStamp;
            GameState newGameState = _clientCalls.GetGameState(this.PlayerUID, currentTimeStamp).AsTask().Result;
            UpdateOtherPlayers(newGameState);
            return newGameState;
       }

        private void UpdateOtherPlayers(GameState newGameState)
        {
            this.OtherPlayers = newGameState.Players.Where(x => x.Id != newGameState.PlayerDTO.Id).ToList();
            PlayersInRoom = OtherPlayers.Where(x => x.CurrentGameRoomId == newGameState.PlayerDTO.CurrentGameRoomId).ToList();
        }

        private void OnTimerTick(object source, EventArgs e)
        {
            _globalTick.SubscribedMembers.Add(this.GetType().Name);
           _currentGameState = GetNextGameState();
        }
    }
}
