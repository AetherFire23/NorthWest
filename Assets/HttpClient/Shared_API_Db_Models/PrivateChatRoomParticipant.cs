using Assets.Utils;
using System;

namespace WebAPI.Models
{
    public class PrivateChatRoomParticipant : IDbKey
    {
        public Guid Key => Id;
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
