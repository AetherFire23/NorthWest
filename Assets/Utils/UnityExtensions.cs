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

    public static T GetComponentSafely<T>(this GameObject self)
    {
        var component = self.GetComponent<T>();
        
        if(component == null)
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

    public static Vector3 ToWorldPoint(this Vector3 self) => Camera.main.ScreenToWorldPoint(self);

    public static bool CompareTag(this RaycastHit2D self, string otherTag) => self.transform.gameObject.CompareTag(otherTag);

    public static void SelfDestroy(this GameObject self)
    {
        GameObject.Destroy(self);
    }

    public static void AddMethod(this Button button , Action newAction)
    {
        button.onClick.AddListener(delegate 
        { 
            newAction(); 
        });
    }

}