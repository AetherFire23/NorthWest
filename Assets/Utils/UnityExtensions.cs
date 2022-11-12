using Assets.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
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

    public static T GetComponentSafely<T>(this GameObject self)
    {
        var component = self.GetComponent<T>();

        if (component == null)
        {
            throw new NotImplementedException($"You tried to load this component : {typeof(T).Name} on this gameobject : {self.name}. It was not found.  ");
        }

        return component;

    }

    public static T GetComponentSafely<T>(this Component self)
    {
        var component = self.GetComponent<T>();

        if (component == null)
        {
            throw new NotImplementedException($"You tried to load this component : {typeof(T).Name} on this gameobject : {self.name}. It was not found.  ");
        }

        return component;

    }

    public static T GetComponentSafely<T>(this MonoBehaviour self)
    {
        var component = self.GetComponent<T>();

        if (component == null)
        {
            throw new NotImplementedException($"You tried to load this component : {typeof(T).Name} on this gameobject : {self.name}. It was not found.  ");
        }

        return component;

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
        button.onClick.AddListener(delegate
        {
            newAction();
        });
    }

    public static void DestroyAndRemoveUGIFromList<TUGI, TAccessScript>(List<TUGI> toRemove, List<TUGI> removeFrom ) where TUGI : InstanceWrapper<TAccessScript>
    {
        foreach(var p in toRemove)
        {
            p.UnityInstance.SelfDestroy();
            removeFrom.Remove(p);
        }
    }
}