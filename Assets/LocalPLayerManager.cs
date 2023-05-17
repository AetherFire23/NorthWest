using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPLayerManager : MonoBehaviour
{
    public Vector3 PlayerPosition
    {
        get { return this.gameObject.transform.position; }
        set { this.gameObject.transform.position = value; }
    }


}