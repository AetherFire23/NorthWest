using Shared_Resources.Entities;
using Shared_Resources.Models;
using System.Collections.Generic;

namespace Assets.HttpStuff
{
    public class GameDataStore : DatastoreBase<GameState>
    {
        public void UpdatePlayers(List<Player> players)
        {
            base.State.Players = players;
        }
    }
}
