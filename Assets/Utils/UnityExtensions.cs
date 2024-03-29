﻿using Assets.Enums;
using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class UnityExtensions
{
    public static Vector3 SetX(this Vector3 self, float x)
    {
        return new Vector3(x, self.y, self.z);
    }
    public static Vector3 SetY(this Vector3 self, float y)
    {
        return new Vector3(self.x, y, self.z);
    }
    public static Vector3 SetZ(this Vector3 self, float z)
    {
        return new Vector3(self.x, self.y, z);
    }
    public static Vector2 SetX(this Vector2 self, float x)
    {
        return new Vector2(x, self.y);
    }
    public static Vector2 SetY(this Vector2 self, float y)
    {
        return new Vector2(self.x, y);
    }

    public static Vector3 GetPosition(this Player player)
    {
        Vector3 position = new Vector3(player.X, player.Y, player.Z);
        return position;
    }

    public static void MoveTowards(this GameObject self, Vector2 to, float speed)
    {
        self.transform.position = Vector3.MoveTowards(self.transform.position, to, speed * Time.deltaTime);
    }

    public static void SetParent(this GameObject self, GameObject parent)
    {
        self.transform.parent = parent.transform;
    }

    public static Vector2 ToVector2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    public static GameObject GetParent(this GameObject self)
    {
        return self.transform.parent.gameObject;
    }

    public static T GetComponentInParent<T>(this GameObject self)
    {
        return self.GetParent().GetComponent<T>();
    }

    public static UnityEngine.Object LoadPrefabSafely(string resourceName)
    {
        var go = Resources.Load(resourceName);
        bool IsNull = go == null;

        if (IsNull)
        {
            throw new NotImplementedException($"The resource you tried to load ({resourceName})  in {NameOfCallingClass()} could not be found. Check if the files in resources folder match the resourceName property.");
        }
        return go;
    }

    public static string NameOfCallingClass()
    {
        string fullName;
        Type declaringType;
        int skipFrames = 2;
        do
        {
            MethodBase method = new StackFrame(skipFrames, false).GetMethod();
            declaringType = method.DeclaringType;
            if (declaringType == null)
            {
                return method.Name;
            }
            skipFrames++;
            fullName = declaringType.FullName;
        }
        while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

        return fullName;
    }

    public static Vector3 WithOffset(this Vector3 self, float xOffset, float yOffset, float zOffset) => new Vector3(self.x + xOffset, self.y + yOffset, self.z + zOffset);
    public static Vector2 WithOffset(this Vector2 self, float xOffset, float yOffset, float zOffset) => new Vector3(self.x + xOffset, self.y + yOffset);
    public static Vector2 ToScreenPoint(this Vector3 self) => Camera.main.WorldToScreenPoint(self);


    public static GameObject CreateInstanceUnderSelf(this GameObject self, GameObject toInstantiate)
    {
        var sexyObject = GameObject.Instantiate(toInstantiate, self.transform);
        return sexyObject;
    }

    public static string GetTextWithoutHiddenCharacters(this TextMeshProUGUI self)
    {
        string meshText = self.text.Trim((char)8203);
        return meshText;
    }

    public static string GetTextWithoutHiddenCharacters(this TMP_InputField self)
    {
        string meshText = self.text.Trim((char)8203);
        return meshText;
    }

    public static float GetAngleFromVector(this Vector2 direction)
    {
        // vector 1,0 parce que dans le fond c<est une DIFFERENCE entre les angles que ca donne. Moi je veux que la direction soit a droite, pour que ca donne 0 quand on est a droite comm un vrai compas.
        return Vector2.SignedAngle(new Vector2(1, 0), direction); // works ! 
    }



    public static Vector3 ToWorldPoint(this Vector3 self) => Camera.main.ScreenToWorldPoint(self);

    public static bool CompareTag(this RaycastHit2D self, string otherTag) => self.transform.gameObject.CompareTag(otherTag);

    public static void SelfDestroy(this GameObject self)
    {
        GameObject.Destroy(self);
    }

    public static Direction GetDirectionsBetweenPositions(Vector2 source, Vector2 target)
    {
        Vector2 direction = GetDirectionVector(source, target);

        float angle = direction.GetAngleFromVector();

        var directionEnum = GetDirectionEnumFromAngle(angle);

        return directionEnum;
    }

    public static Direction GetDirectionEnumFromAngle(float angle) // signed angle
    {
        bool isWest = angle <= -136 && angle <= 180
              || angle <= 180 && angle >= 136;
        if (isWest) return Direction.West;

        bool isEast = angle >= -45 && angle <= 45;
        if (isEast) return Direction.East;

        bool isSouth = angle >= -135 && angle <= -46;
        if (isSouth) return Direction.South;

        bool isNorth = angle >= 46 && angle <= 135;
        if (isNorth) return Direction.North;

        return Direction.East;
    }

    public static Vector2 GetDirectionVector(Vector2 source, Vector2 target)
    {
        return target - source;
    }

    public static void AddMethod(this Button button, Action newAction)
    {
        //UnityEngine.Debug.Log($"Just executed action {newAction.GetType().DeclaringType}");
        button.onClick.AddListener(delegate
        {
            newAction();
        });
    }

    public static bool IsWithinBounds(Vector2 startPosition, Vector2 maxPosition, Vector2 pointPosition)
    {
        bool isOverStartPoint = startPosition.x < pointPosition.x && startPosition.y < pointPosition.y;
        bool isUnderEndPoint = maxPosition.x > pointPosition.x && maxPosition.y > pointPosition.y;

        bool isContained = isOverStartPoint && isUnderEndPoint;

        return isContained;
    }

    public static List<T> GetEnumValues<T>() where T : Enum
    {
        var allEnumTypes = (T[])Enum.GetValues(typeof(T));
        return allEnumTypes.ToList();
    }

    public static void ThrowAnyNull<T>(this IEnumerable<T> sequence, Func<T, string> logMessageFunc = null)
        where T : class // reference type constraint
    {
        var nullItems = sequence
            .Select((item, index) => (Item: item, Index: index))
            .Where(x => x.Item == null)
            .ToList();

        foreach (var (item, index) in nullItems)
        {
            string logMessage = logMessageFunc != null ? logMessageFunc(item) : $"Null item at index {index}: {item}";
            UnityEngine.Debug.Log(logMessage);
        }

        throw new ArgumentNullException($"An element was null in list {sequence}");
    }

    /// <summary> Needs to be a delegate with syntax: async() => await task and have 0 idea why 
    /// so to be clear, NOT AddTaskFunc(Task) but the other syntax 
    /// apres reflexion je pense que cest pcq il peut pas avoir de parameter pour que ca soit generic 
    /// </summary>
    /// 
    public static void AddTaskFunc(this Button self, Func<UniTask> task)
    {
        self.onClick.AddListener(async () => await task.Invoke());
    }

    public static Func<UniTask> ToDelegate(this UniTask task)
    {
        Func<UniTask> z = async () => await task;
        return z;
    }

    public static IEnumerable<T2> GetAppearedEntities<T, T2>(IEnumerable<T> oldEntities, IEnumerable<T2> upToDateEntities)
        where T : PrefabScriptBase, IEntity
        where T2 : IEntity
    {
        var oldEntityIds = oldEntities.Select(x => x.Id);
        var appeared = upToDateEntities.Where(x => !oldEntityIds.Contains(x.Id));
        return appeared;
    }

    public static IEnumerable<T> GetDisappearedGameObjects<T, T2>(IEnumerable<T> oldEntities, IEnumerable<T2> upToDateEntities)
        where T : PrefabScriptBase, IEntity
         where T2 : IEntity
    {
        var upToDateEntityIds = upToDateEntities.Select(x => x.Id);
        var disappeared = oldEntities.Where(x => !upToDateEntityIds.Contains(x.Id));
        return disappeared;
    }

    /// <summary>
    /// Works with async () => await syntax too !
    /// </summary>
    public static void AddDropDownAction(this TMP_Dropdown self, Action action)
    {
        self.onValueChanged.AddListener(delegate
        {
            action();
        });
    }

    public static T FindUniqueMonoBehaviour<T>() where T : MonoBehaviour
    {
        List<T> monoBehaviours = (GameObject.FindObjectsOfType<T>()).ToList();

        if (!monoBehaviours.Any()) throw new Exception($"Not found:{typeof(T)}");
        if (monoBehaviours.Count > 1) throw new Exception($"Too many of:{typeof(T)}");

        return monoBehaviours.First();
    }

    public static List<T> FindAllSubclassesOf<T>() where T : class
    {
        MonoBehaviour[] monoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();
        var monos = monoBehaviours
            .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
            .Select(x => x as T).ToList();

        if (!monos.Any()) throw new Exception($"No monos of type {typeof(T)}");

        return monos;
    }

    public static void SetTransparent(this TMPro.TextMeshProUGUI textMeshProUGUI)
    {
        textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, 0f);
    }

    public static void RemoveTransparency(this TMPro.TextMeshProUGUI textMeshProUGUI)
    {
        textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, 1f);
    }

    public static void SetVisibleAlphaLayer(this TMPro.TextMeshProUGUI textMeshProUGUI, bool isVisible)
    {
        if (isVisible)
        {
            textMeshProUGUI.RemoveTransparency();
        }
        else
        {
            textMeshProUGUI.SetTransparent();
        }
    }

    public static TextMeshProUGUI GetTextMesh(this Button self)
    {
        var text = self.GetComponentInChildren<TextMeshProUGUI>();
        return text;
    }


    // va falloir faire une liste de components ou dictionary de component, oldValue
    // serait nice de aps faire de canvas pis de pouvoir toute crisser invisible
    //public static List<Type> AlphaLayerTypes = new List<Type>()
    //{
    //    typeof(TextMeshProUGUI),
    //    typeof()
    //};
    //public static void SetVisibleAlphaLayerAndChildren(this GameObject self, bool isVisible)
    //{


    //}
}