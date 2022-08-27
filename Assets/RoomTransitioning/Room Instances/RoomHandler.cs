using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace Assets.RoomTransitioning.Room_Instances
{
    public class RoomHandler : IInitializable
    {
        // public List<GameObject> RoomObjects = new List<GameObject>(); // null list was problemativ
        public List<RoomObject> Rooms = new List<RoomObject>();

        public RoomHandler(FirstRoomObject first, SecondRoomObject second) // must inject all new rooms here 
        {
            // comment adder une room :
            // Faire une Room, qui inherit de RoomObject
            // hardcode le centerpoint 
            // harcode le roomtype 
            // Faire une RoomEntry dans le SetupAdjacentConfigs

            Rooms.Add(first);
            Rooms.Add(second);
            ConnectNeighbors();
        }

        public void Initialize()
        {

        }

        public void ConnectNeighbors()
        {
            var adjacentConfigs = SetupRoomsConfigurations();
            foreach(RoomObject room in this.Rooms) // Pour toute les rooms qui existent, ajoute
            {
                RoomType currentRoomType = room.roomType;
                List<RoomType> adjacentRoomTypes = adjacentConfigs.GetValueOrDefault(currentRoomType);
                Connect(room, adjacentRoomTypes);
            }
                                }

        public void Connect(RoomObject room, List<RoomType> neighborTypes)
        {
            RoomType currentRoomType = room.roomType;
            List<RoomObject> adjacentRooms = this.Rooms.Where(room => neighborTypes.Contains(room.roomType)).ToList();
            
            foreach(RoomObject neighbor in adjacentRooms)
            {
                room.AddNeighbor(neighbor.roomType, neighbor);
            }
        }

        public Dictionary<RoomType, List<RoomType>> SetupRoomsConfigurations() // 
        {
            Dictionary<RoomType, List<RoomType>> adjacentRoomTypes = new Dictionary<RoomType, List<RoomType>>();

            //setup les rooms.
            var firstRoom = new RoomEntry(RoomType.Start, RoomType.Second);
            adjacentRoomTypes.Add(firstRoom.RoomType, firstRoom.accessibleRoomTypes);

            var secondRoom = new RoomEntry(RoomType.Second, RoomType.Start);
            adjacentRoomTypes.Add(secondRoom.RoomType, secondRoom.accessibleRoomTypes);

            return adjacentRoomTypes;
        }
    }

    public class RoomEntry
    {
        public RoomType RoomType;
        public List<RoomType> accessibleRoomTypes = new List<RoomType>();

        public RoomEntry(RoomType roomType, RoomType firstAdjacent)
        {
            RoomType = roomType;
            accessibleRoomTypes.Add(firstAdjacent);
        }

        public RoomEntry(RoomType roomType, RoomType firstAdjacent, RoomType secondAdjacent)
        {
            RoomType = roomType;
            accessibleRoomTypes.Add(firstAdjacent);
            accessibleRoomTypes.Add(secondAdjacent);
        }

        public RoomEntry(RoomType roomType, RoomType firstAdjacent, RoomType secondAdjacent, RoomType thirdAdjacent)
        {
            RoomType = roomType;
            accessibleRoomTypes.Add(firstAdjacent);
            accessibleRoomTypes.Add(secondAdjacent);
            accessibleRoomTypes.Add(thirdAdjacent);
        }
    }
}