using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RoomTransitioning.Room_Instances
{
    public class SecondRoomObject : RoomObject
    {
        public SecondRoomObject(SecondRoomScript secondRoomScript) : base(secondRoomScript)
        {
            base.CenterPosition = new Vector3(-8, -10, 0);
            base.roomType = RoomType.Second;
        }
    }
}
