using Assets.OtherPlayers;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets
{
    public class OtherCharactersManager : ITickable // OtherCharacterBehaviour
    {
        private MainPlayer _mainPlayer;
        private ClientCalls _clientCalls;
        public List<OtherPlayerInstance> players = new List<OtherPlayerInstance>();
        private OtherCharactersObjectContainer _otherCharactersObjectContainer;

        public OtherCharactersManager(MainPlayer mainPlayer,
            ClientCalls clientCalls,
            OtherCharactersObjectContainer otherCharactersObjectContainer)
        {
            _mainPlayer = mainPlayer;
            _clientCalls = clientCalls;
            _otherCharactersObjectContainer = otherCharactersObjectContainer;
        }

        public void Tick()
        {

        }

        public void InstantiateOtherCharacters()
        {
            var playerModels = GetOtherPlayers().Result;

            foreach (global::PlayerModel player in playerModels)
            {
                var newOtherPlayerInstance = new OtherPlayerInstance(_otherCharactersObjectContainer.GameObject);
                newOtherPlayerInstance.InstanceBehaviour.textComp.text = player.Name;
                newOtherPlayerInstance.UnityInstance.name = player.Name;
                newOtherPlayerInstance.UnityInstance.gameObject.transform.position = player.GetPosition();
            }
        }

        public async Task<List<global::PlayerModel>> GetOtherPlayers()
        {
            var players = await _clientCalls.GetPlayers();
            return players.Where(player => player.Id != _mainPlayer.playerModel.Id).ToList(); // excludes main player
        }
    }
}