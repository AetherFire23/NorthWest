using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WebAPI.Enums;

public class Player : IDbKey
{
    public Guid Key => Id;
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid CurrentChatRoomId { get; set; }
    public Guid CurrentGameRoomId { get; set; }
    public ProfessionType Profession { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public string Name { get; set; }
    public int HealthPoints { get; set; }
    public int ActionPoints { get; set; }

    public Vector3 GetPosition()
    {
        return new Vector3(X, Y, Z);
    }

    public Player Clone()
    {
        return new Player()
        {
            Id = this.Id,
            ActionPoints = this.ActionPoints,
            X = this.X,
            Y = this.Y,
            Z = this.Z,
            CurrentChatRoomId = this.CurrentChatRoomId,
            CurrentGameRoomId = this.CurrentGameRoomId,
            GameId = this.GameId,
            HealthPoints = this.HealthPoints,
            Name = this.Name,
            Profession = this.Profession
        };
    }
}

