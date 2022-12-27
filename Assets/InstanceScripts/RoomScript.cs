using Assets.Room_Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebAPI.Models.DTOs;

public class RoomScript : MonoBehaviour
{
    public RoomTemplate DbRoom = new RoomTemplate();
    public string RoomName;
    public RoomDTO RoomDTO;
}
