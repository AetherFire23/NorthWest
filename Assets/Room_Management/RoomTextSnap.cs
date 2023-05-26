using TMPro;
using UnityEngine;

public class RoomTextSnap : MonoBehaviour
{
    [SerializeField] Transform _roomPosition;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float yoff;
    [SerializeField] RoomInfoScript _roomInfoScript;
    private void Update()
    {
        var toWorld = Camera.main.WorldToScreenPoint(transform.position);
        _text.transform.position = toWorld;
        _text.text = _roomInfoScript.TargetRoomName;
    }
}
