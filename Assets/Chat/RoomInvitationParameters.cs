using System;

namespace Assets.CHATLOG3
{
    public class RoomInvitationParameters
    {
        public string PlayerName { get; set; } // not sent to server
        public Guid FromId { get; set; }
        public Guid TargetPlayer { get; set; }
        public Guid TargetRoomId { get; set; }
    }
}
