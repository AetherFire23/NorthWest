using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;
namespace Assets
{
    public class PlayerPositionTick : ITickable
    {
        public SimpleManager _simpleManager;
        private PlayerScript _playerFacade;
        private MainPlayer _mainPlayer;
        private ClientCalls _clientCalls;
        private OtherCharactersManager _otherCharactersManager;
        SimpleTimer timer = new SimpleTimer(0.2f);

        public PlayerPositionTick(SimpleManager simpleManager, 
            PlayerScript facade, 
            MainPlayer mainPlayer, 
            ClientCalls clientCalls, 
            OtherCharactersManager otherCharactersManager)
        {
            _simpleManager = simpleManager;
            _playerFacade = facade;
            _mainPlayer = mainPlayer;
            _clientCalls = clientCalls;
            _otherCharactersManager =
            _otherCharactersManager = otherCharactersManager;
        }

        public void Tick()
        {
            if (!_simpleManager.HasInitializedGame) return; // do not spawn if not ready
            timer.AddTime(Time.deltaTime); // if game is initialized, add time to the timer

            if (!timer.HasTicked) return; // must wait for timer to finish before updated positions 
            timer.Reset();

            UpdateOtherCharactersPosition();

            PushLocalPlayerPositionToServer();
        }

        public void PushLocalPlayerPositionToServer()
        {
            //Pusher notre currentposition au serveur
            PlayerModel toUpdate = _mainPlayer.playerModel; // pour acceer au bon GUI 
            toUpdate.X = _playerFacade.Position.x;
            toUpdate.Y = _playerFacade.Position.y;
            toUpdate.Z = _playerFacade.Position.z;
            _clientCalls.UpdatePosition(toUpdate);
        }

        public void UpdateOtherCharactersPosition()
        {
            List<PlayerModel> players = _clientCalls.GetPlayers().AsTask().Result;

            foreach (var apiPlayer in players)
            {
                foreach (var instancePlayer in _otherCharactersManager.players)
                {
                    if (instancePlayer.model.Id == apiPlayer.Id)
                    {
                        // SetTargetPoint
                        //instancePlayer.instance.script.TargetPoint = apiPlayer.GetPosition();
                        // instancePlayer.instance.UnityInstance.transform.position = apiPlayer.GetPosition();
                    }
                }
            }
        }
    }
}
