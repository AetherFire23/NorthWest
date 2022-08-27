using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameState_Management.Models
{
    public class PrivateChatRoomParticipant
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
