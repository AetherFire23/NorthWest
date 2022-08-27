﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using Assets.GameState_Management.Models;
using Cysharp.Threading.Tasks;
using Assets.ChatLog_Manager;

namespace Assets.GameState_Management
{
    public class GameStateManager : ITickable, IInitializable
    {
        public List<PrivateChatRoomParticipant> ParticipantsInPlayerRooms => _currentGameState.PrivateChatRooms;
        public List<MessageModel> NewMessages => _currentGameState.NewMessages ?? new();
        public List<PlayerModel> Players => _currentGameState.Players ?? new();
        public bool HasTicked => _tickTimer.HasTicked;

        private GameState _currentGameState;
        private readonly SimpleTimer _tickTimer = new SimpleTimer(3f);
        private readonly ClientCalls _clientCalls;
        private readonly MainPlayer _mainPlayer;

        public GameStateManager(ClientCalls clientCalls, MainPlayer mainPlayer)
        {
            _clientCalls = clientCalls;
            _mainPlayer = mainPlayer;
        }

        public void Initialize()
        {
            _currentGameState = InitializeGameState();
        }

        public void Tick()
        {
            if (_tickTimer.HasTicked) // event for everytime it ticks is needed
            {
                Debug.Log("The GameState timer has ticked");
                _tickTimer.Reset();
                _currentGameState = GetNextGameState();
            }
            _tickTimer.AddTime(Time.deltaTime);
        }

        private GameState InitializeGameState() // leaves timeStamp at null so I can initialize the whole thing instead of updating
        {
            DateTime? currentTimeStamp = null;
            var firstGameState = _clientCalls.GetGameState(_mainPlayer.Id, currentTimeStamp).AsTask().Result;
            return firstGameState;
        }

        private GameState GetNextGameState()
        {
            DateTime? currentTimeStamp = _currentGameState.TimeStamp;
            GameState newGameState = _clientCalls.GetGameState(_mainPlayer.Id, currentTimeStamp).AsTask().Result;
            return newGameState;
        }
    }
}
