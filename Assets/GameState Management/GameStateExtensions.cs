//using Cysharp.Threading.Tasks;
//using Shared_Resources.DTOs;
//using Shared_Resources.Entities;
//using Shared_Resources.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Assets.GameState_Management
//{
//    public static class GameStateExtensions
//    {
//        public static List<Player> GetPlayersInChatRoom(this GameState gameState, Guid roomId)
//        {
//            if (roomId.Equals(gameState.GameId))
//            {
//                return gameState.Players;
//            }

//            var chatRooms = gameState.PrivateChatRoomParticipants.Where(x => x.RoomId.Equals(roomId));
//            var players = gameState.Players.Join(chatRooms,
//                player => player.Id,
//                room => room.ParticipantId,
//                (p, r) => p).ToList();

//            return players;
//        }

//        public static List<Guid> GetDistinctChatRooms(this GameState gameState)
//        {
//            var chatRooms = gameState.PrivateChatRoomParticipants
//                .Select(x => x.RoomId)
//                .Distinct();
//            return chatRooms.ToList();
//        }

//        public static List<Message> GetMessagesInChatRoom(this GameState gameState, Guid chatRoomId)
//        {
//            var messages = gameState.NewMessages.Where(x => x.RoomId.Equals(chatRoomId)).ToList();
//            return messages;
//        }

//        public static List<Item> GetItemsInRoom(this GameState gameState, string roomName)
//        {
//            var room = gameState.Rooms.FirstOrDefault(x => x.Name.Equals(roomName));
//            if (room == null) throw new ArgumentException($"Could not find {roomName} in gamestate.");

//            var items = room.Items.ToList();
//            return items;
//        }

//        public static RoomDTO GetRoomById(this GameState gameState, Guid id)
//        {
//            var room = gameState.Rooms.FirstOrDefault(x => x.Id == id);
//            if (room is null) throw new Exception("It broke");

//            return room;
//        }

//        public static RoomDTO GetRoomByName(this GameState gameState, string roomName)
//        {
//            var room = gameState.Rooms.FirstOrDefault(x => x.Name.Equals(roomName));
//            if (room is null) throw new Exception($"ROom not found: {roomName} ");

//            return room;
//        }

//        public static List<RoomDTO> GetLandmassRooms(this GameState gameState)
//        {
//            var landmassRooms = gameState.Rooms.Where(x => x.IsLandmass).ToList();
//            return landmassRooms;
//        }

//        public static List<RoomDTO> GetRoomsInAlphabeticalOrder(this GameState gameState)
//        {
//             var rooms = gameState.Rooms.OrderBy(x=> x.Name).ToList();
//            return rooms;
//        }
//    }
//}
