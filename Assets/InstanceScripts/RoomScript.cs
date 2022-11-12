using Assets.Room_Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomScript : MonoBehaviour
{
    public RoomTemplate DbRoom = new RoomTemplate();
    public string RoomName;
}
