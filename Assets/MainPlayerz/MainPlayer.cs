//using Assets.GameState_Management;
//using Assets.MainMenu;
//using Cysharp.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using WebAPI.Models.DTOs;
//using Zenject;

//namespace Assets
//{
//    public class MainPlayer : ITickable, IInitializable // je devrais utiliser le inheritance de UnityObject pour gerer linstance et le script
//    { // cette classe en realite est desuete et faudrait utiliser le localplayerDTO. Faudrait caller le localplayer dans le gamestate en fait


//        //public Player playerModel;

//        public PlayerDTO playerDTO; // initialized through gamestate
//        public Guid PlayerUID { get; set; }
//        public RoomType CurrentRoomType = RoomType.Start;
//        public Guid Id => playerDTO.Id;
//        public Guid GameId => playerDTO.GameId;

//        private readonly ClientCalls _clientCalls;

//        public MainPlayer(ClientCalls clientCalls)
//        {

//            Debug.Log("Loading MainPlayer...");
//            MainMenuPersistence pers = Resources.Load<MainMenuPersistence>("mainMenuPersistence");

//            if (pers == null)
//            {
//                throw new Exception("No player UID found from main menu persistence scriptable object.");
//            }

//            this.PlayerUID = pers.MainPlayerId;

//            Debug.Log("Finished loading MainPlayer");
//        }
//        public void Initialize()
//        {

//        }

//        public void Tick()
//        {

//        }
//    }
//}