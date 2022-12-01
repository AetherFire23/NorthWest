

using System;

namespace WebAPI.Models
{
    public class PrivateInvitation
    {
        public Guid Id { get; set; }
        public Guid FromPlayerId { get; set; } // localPlayer
        public Guid RoomId { get; set; }

        public string FromPlayerName { get; set; }
        public Guid ToPlayerId { get; set; }
        public string ToPlayerName { get; set; } // invitedUser
        public bool IsAccepted { get; set; }
        public bool RequestFulfilled { get; set; }

        //public Guid Id { get; set; }
        //public Guid FromUserId { get; set; }
        //public string FromUserName { get; set; }
        //public Guid RoomId { get; set; }
        //public Guid ToUserId { get; set; }
        //public string ToUserName { get; set; }
        //public bool IsAccepted { get; set; }
        //public bool RequestFulfilled { get; set; }
    }
}
