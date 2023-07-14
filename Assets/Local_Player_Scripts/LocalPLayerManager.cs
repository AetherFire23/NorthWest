using UnityEngine;

public class LocalPLayerManager : MonoBehaviour
{
    public Vector3 PlayerPosition
    {
        get { return this.gameObject.transform.position; }
        set { this.gameObject.transform.position = value; }
    }

    public float X => PlayerPosition.x;
    public float Y => PlayerPosition.y;
    public float Z => PlayerPosition.z;
}
