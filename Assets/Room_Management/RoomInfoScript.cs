using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfoScript : MonoBehaviour
{
    // just change roomname and the hnadler will do anything
    // It is the handler that add all the rooms to a list
    // pourrais faire un script qui snap au gameObjectName...
    [SerializeField] public string TargetRoomName;
}
