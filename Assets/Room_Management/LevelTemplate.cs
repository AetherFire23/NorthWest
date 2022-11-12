using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Room_Management
{
    public class LevelTemplate // doit etre EQual au model trouve dans le webapi
    {
        public RoomTemplate Kitchen1 { get; set; }
        public RoomTemplate Kitchen2 { get; set; }
        public RoomTemplate EntryHall { get; set; }

    }
}
