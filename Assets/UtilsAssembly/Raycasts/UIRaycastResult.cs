using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRaycastResult
{
    public List<RaycastResult> raycastResults;
    public bool HasFoundHit;
    public RaycastResult? FirstResult;
    public GameObject GameObject;
}
