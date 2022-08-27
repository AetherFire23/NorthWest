using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ChatLog_Manager
{
    public class PrivateInvitation
    {
        // [Key]
        public Guid Id { get; set; }
        public Guid FromPlayerId { get; set; }
        public string FromPlayerName { get; set; }
        public Guid RoomId { get; set; }
        public Guid ToPlayerId { get; set; }
        public string ToPlayerName { get; set; } 
        public bool IsAccepted { get; set; }
        public bool RequestFulfilled { get; set; }
    }
}