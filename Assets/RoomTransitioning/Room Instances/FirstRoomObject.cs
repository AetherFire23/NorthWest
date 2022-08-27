using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
namespace Assets.RoomTransitioning.Room_Instances
{
    public class FirstRoomObject : RoomObject
    {
        
        public FirstRoomObject(FirstRoomScript script) : base(script) // is injected and passed to base
        {
            base.CenterPosition = new Vector3(-6,0,0);
            base.roomType = RoomType.Start;
        }
    }
}
