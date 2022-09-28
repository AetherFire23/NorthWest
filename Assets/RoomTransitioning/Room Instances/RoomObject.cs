using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RoomTransitioning.Room_Instances
{
    public abstract class RoomObject : UGFWrapper
    {
        public RoomType roomType = RoomType.Start; // doit etre hardcoded
        public Vector3 CenterPosition = new Vector3(0, 0, 0); // must be hardcoded in inherited class
        public Dictionary<RoomType, RoomObject> adjacentRooms = new Dictionary<RoomType, RoomObject>(); // initialized through RoomHandler because of DI injection avec un dictionnaire
        public RoomObject(MonoBehaviour script) : base(script)
        {
        }

        public void AddNeighbor(RoomType neighborRoomType, RoomObject adjacentRoom)
        {
            this.adjacentRooms.Add(neighborRoomType, adjacentRoom);
        }

    }
}
