using Shared_Resources.Entities;
using Shared_Resources.Models;
using System;
using System.Linq;

namespace Assets.Utils
{
    public static class MainMenuStateExtensions
    {
        public static Player FindPlayerFromGameId(this MainMenuState self, Guid gameId)
        {
            Player userId = self.UserDto.Players.First(x => x.GameId == gameId);
            return userId;
        }
    }
}
