using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using System.Collections;
using System.Collections.Generic;
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
