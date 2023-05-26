using UnityEngine;

public class PrefabScriptBase : MonoBehaviour
{
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
}
