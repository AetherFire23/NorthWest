using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public static class UIRaycast
{
    public static UIRaycastResult MouseRaycastResult(Func<RaycastResult, bool> whereFilter)
    {
        return new UIRaycastResult()
        {
            raycastResults = UIRaycast.RawUIRaycast().Where(whereFilter).ToList(),
        };
    }
    //private static List<UnityEngine.EventSystems.RaycastResult> RawUIRaycast()
    //{
    //    PointerEventData eventData = new PointerEventData(EventSystem.current);
    //    eventData.position = Input.mousePosition;
    //    List<UnityEngine.EventSystems.RaycastResult> raycastResult = new List<UnityEngine.EventSystems.RaycastResult>();
    //    EventSystem.current.RaycastAll(eventData, raycastResult);
    //    return raycastResult;
    //}

    private static List<UnityEngine.EventSystems.RaycastResult> RawUIRaycast()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();
        List<UnityEngine.EventSystems.RaycastResult> raycastResult = new List<UnityEngine.EventSystems.RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResult);
        return raycastResult;
    }
}