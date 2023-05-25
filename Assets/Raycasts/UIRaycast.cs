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
        var results = UIRaycast.RawUIRaycast().Where(whereFilter).ToList();
        return new UIRaycastResult()
        {
            raycastResults = results,
            HasFoundHit = results.Any(),
            FirstResult = results.Any() ? results.First() : null,
            GameObject = results.Any() ? results.First().gameObject : null,
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

    public static List<UnityEngine.EventSystems.RaycastResult> RawUIRaycast()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();
        List<UnityEngine.EventSystems.RaycastResult> raycastResult = new List<UnityEngine.EventSystems.RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResult);
        return raycastResult;
    }

    public static T ScriptOrDefault<T>() where T : MonoBehaviour
    {
        var raycastResults = RawUIRaycast();
        var results = raycastResults.Where(x => x.gameObject.GetComponent<T>() is not null);

        if (!results.Any()) return null;

        var first = results.First().gameObject.GetComponent<T>();
        return first;
    }

    public static bool Any()
    {
        bool result = RawUIRaycast().Any();
        return result;
    }

    public static bool Any(Func<RaycastResult, bool> filter)
    {
        foreach (var result in RawUIRaycast())
        {
            if (filter(result)) return true;
        }
        return false;
    }

    public static bool TagExists(string tag)
    {
        var res = UIRaycast.RawUIRaycast();
        var any = res.Any(x => x.gameObject.CompareTag(tag));

        return any;
    }
}