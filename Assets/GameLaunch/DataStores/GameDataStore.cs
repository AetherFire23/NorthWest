using Assets.GameState_Management;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using Shared_Resources.Models.SSE;
using System.Collections.Generic;
using System.Linq;

namespace Assets.HttpStuff
{
    public class GameDataStore : DatastoreBase<GameState>
    {
        public void UpdatePlayers(List<Player> players)
        {
            base.State.Players = players;
        }

        public void UpdateItems(PlayerAndRoomItems playerAndRoomItems) // remplacer une liste 
        {
            State.PlayerDTO.Items = playerAndRoomItems.PlayerItems;

            foreach (var room in State.Rooms) // gotta look for every room lol
            {
                room.Items.Clear();

                var roomItems = playerAndRoomItems.RoomItems.Where(x => x.OwnerId == room.Id).ToList();

                if (!roomItems.Any()) continue;

                room.Items.AddRange(roomItems);
            }
        }

        public void UpdateLocalPlayerRoomId(string roomName)
        {
            var roomId = State.GetRoomByName(roomName).Id;
            State.PlayerDTO.CurrentGameRoomId = roomId;
        }
    }
}
