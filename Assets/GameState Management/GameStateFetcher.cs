//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Zenject;
//using UnityEngine;
//using Cysharp.Threading.Tasks;
//using Assets.ChatLog_Manager;
//using Assets.Room_Management;
//using Assets.MainMenu;
//using Assets.Enums;
//using Shared_Resources.Entities;
//using Shared_Resources.DTOs;
//using Shared_Resources.Models;
//using Shared_Resources.Enums;
//using Shared_Resources.GameTasks;
//using Assets.GameLaunch;

//namespace Assets.GameState_Management
//{
//    public class GameStateFetcher : MonoBehaviour
//    {
//        [SerializeField]
//        //private Client _client;

//        private async UniTask<GameState> GetFirstGameState(Guid playerId) // leaves timeStamp at null so I can initialize the whole thing instead of updating
//        {
//            DateTime? currentTimeStamp = null;
//            GameState firstGameState = await _client.GetGameState(playerId, currentTimeStamp);
//            return firstGameState;
//        }

//        public async UniTask<GameState> GetNextGameState(Guid playerId, DateTime? currentTimeStamp)
//        {
//            GameState newGameState = await _client.GetGameState(playerId, currentTimeStamp);
//            return newGameState;
//        }
//    }
//}
