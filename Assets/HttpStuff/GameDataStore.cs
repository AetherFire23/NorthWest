﻿using Shared_Resources.Entities;
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

            foreach (var room in State.Rooms)
            {
                room.Items.Clear();
                
                var roomItems = playerAndRoomItems.RoomItems.Where(x => x.Id == room.Id).ToList();
                room.Items.AddRange(roomItems);
            }
        } 

        public void DeleteItems()
        {

        }
    }
}
