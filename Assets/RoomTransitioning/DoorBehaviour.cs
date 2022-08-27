using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] public RoomType RoomAccessType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // faudrait jinstalle le 2d raycast 
        //ApiCall.SwitchRoomTo
        //

        bool canSwitchRoom = true;

        if(canSwitchRoom)
        {

        }
    }
}
