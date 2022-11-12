using Assets.ChatLog_Manager;
using System;
using System.Collections.Generic;
using WebAPI.Models;
using WebAPI.Models.DTOs;
using System.Collections.Generic;

namespace WebAPI.GameState_Management
{
    public class GameState
    {
        public PlayerDTO PlayerDTO { get; set; }
        public RoomDTO Room { get; set; }
        public List<Message> NewMessages { get; set; }
        public List<Player> Players { get; set; }
        public List<PrivateInvitation> Invitations { get; set; }
        public List<PrivateChatRoomParticipant> PrivateChatRooms { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
