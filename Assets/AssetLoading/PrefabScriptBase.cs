using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;

public class PrefabScriptBase : MonoBehaviour
{
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
}
