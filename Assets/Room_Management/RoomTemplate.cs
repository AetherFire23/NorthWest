using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Room_Management
{
    public class RoomTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> AdjacentRoomIds { get; set; }
        public List<string> AdjacentNames { get; set; }


        public RoomTemplate Clone()
        {
            RoomTemplate newRoom = new RoomTemplate()
            {
                AdjacentNames = this.AdjacentNames,
                AdjacentRoomIds = this.AdjacentRoomIds,
                Id = this.Id,
                Name = this.Name,
            };
            return newRoom;
        }
    }
}
