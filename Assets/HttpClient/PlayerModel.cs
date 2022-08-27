using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerModel
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid CurrentChatRoomId { get; set; }

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public string Name { get; set; }
    public RoomType CurrentRoom { get; set; }

    public int HealthPoints { get; set; }
    public int ActionPoints { get; set; }

    public Vector3 GetPosition()
    {
        return new Vector3(X, Y, Z);
    }
}

