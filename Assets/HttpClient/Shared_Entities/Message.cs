using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ChatLog_Manager
{
    public class Message : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public Guid GameId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime? Created { get; set; }
    }
}