using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRaycastResult
{
    public List<RaycastResult> raycastResults;
    public bool HasFoundHit => raycastResults.Any();
    public RaycastResult FirstResult => raycastResults.First();
    public GameObject GameObject => FirstResult.gameObject;
}
