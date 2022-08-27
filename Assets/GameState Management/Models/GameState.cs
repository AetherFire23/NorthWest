using Assets.ChatLog_Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameState_Management.Models
{
    public class GameState
    {
        public List<MessageModel> NewMessages { get; set; }
        public PlayerModel Player { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<PrivateInvitation> Invitations { get; set; }
        public List<PrivateChatRoomParticipant> PrivateChatRooms { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}