using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogObjects : MonoBehaviour
{
    [SerializeField] public Button OpenLogButton;
    [SerializeField] public Button ViewAllButton;
    [SerializeField] public Canvas LogCanvas;
    [SerializeField] public RectTransform LogContentTransform;
    [SerializeField] public RectTransform PlayerFilterContentTransform;
    [SerializeField] public RectTransform RoomFilterContentTransform;
}
