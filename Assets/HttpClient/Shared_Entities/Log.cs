using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HttpClient.Shared_Entities
{
    public class Log : IEntity
    {
        public Guid Id { get; set; }
        public Guid TriggeringPlayerId { get; set; } // for private logs
        public Guid RoomId { get; set; }
        public bool IsPublic { get; set; }
        public string EventText { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
    }
}
