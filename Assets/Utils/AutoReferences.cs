//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
//using System.Linq;
//using System;
//using UnityEngine.UI;

//public class AutoReferences : MonoBehaviour
//{
//    [SerializeField] private GameObject _prefab;
//    [SerializeField] private string _fileName;
//    [SerializeField] private bool _IsOpened;
//    private List<string> _propertyLines = new List<string>();

//    void Start()
//    {
//        GameObjectProfile prefabProfile = new GameObjectProfile(_prefab);

//        _propertyLines.Add("// Automatically generated code");
//        FillPropertyLines(prefabProfile);
//        _propertyLines.Add("// End of automatically generated code");
//        _propertyLines.Add("");

//        _propertyLines.Add("void Start()");
//        _propertyLines.Add("{");
//        _propertyLines.Add("// Automatically generated code");
//        _propertyLines.Add($"{prefabProfile.Name} = this.transform.gameObject;");
//        string firstCallParentName = $"{prefabProfile.Name}";
//        FillInitializeContent(prefabProfile, firstCallParentName, true);
//        _propertyLines.Add("// End of automatically generated code");
//        _propertyLines.Add("}");

//        File.WriteAllLines(_fileName, _propertyLines);
//        if (_IsOpened)
//        {
//            System.Diagnostics.Process.Start(_fileName);
//        }
//    }

//    private void FillInitializeContent(GameObjectProfile profile, string parentName, bool isFirstcall)
//    {
//        if (!isFirstcall)
//        {
//            string newInitializeGameObjectPropertyLine = profile.GetGameObjectInitializeLine(parentName);
//            _propertyLines.Add(newInitializeGameObjectPropertyLine);
//        }

//        foreach (var comp in profile.ComponentsProfiles)
//        {
//            string newComponentInitializeName = comp.GetComponentInitializeLine();
//            _propertyLines.Add(newComponentInitializeName);
//        }

//        if (profile.ChildrenProfiles.Count == 0) return;

//        foreach (var prof in profile.ChildrenProfiles)
//        {
//            _propertyLines.Add("");
//            FillInitializeContent(prof, profile.Name, isFirstcall = false); // recursion
//        }
//    }

//    private void FillPropertyLines(GameObjectProfile profile)
//    {
//        string newGameObjectPropertyLine = profile.GetGameObjectPropertyLine();
//        _propertyLines.Add(newGameObjectPropertyLine);

//        foreach (var comp in profile.ComponentsProfiles)
//        {
//            string newComponentPropertyLine = comp.GetComponentPropertyLine();
//            _propertyLines.Add(newComponentPropertyLine);
//        }

//        if (profile.ChildrenProfiles.Count == 0) return;

//        foreach (var prof in profile.ChildrenProfiles)
//        {
//            _propertyLines.Add("");
//            FillPropertyLines(prof); //recursion
//        }
//    }
//}

//public class GameObjectProfile
//{
//    public GameObject GameObject;
//    public string Name;
//    public List<GameObjectProfile> ChildrenProfiles;
//    public List<ComponentProfile> ComponentsProfiles;

//    private const string _publicPropertyPrefix = "public"; // I want the specific type here
//    private const string _gameobjectPrefix = "GameObject";

//    public GameObjectProfile(GameObject go)
//    {
//        this.GameObject = go;
//        this.Name = go.name;
//        this.ComponentsProfiles = CreateComponentProfiles(this.GameObject);
//        this.ChildrenProfiles = GetGameObjectProfiles(go);
//    }

//    public string GetGameObjectInitializeLine(string parentName)
//    {
//        string initializeLine = $"{this.Name} = {parentName}.transform.Find(\"{this.Name}\").gameObject;";
//        return initializeLine;
//    }

//    public string GetGameObjectPropertyLine()
//    {
//        string gameobjectPropLine = $"{_publicPropertyPrefix} {_gameobjectPrefix} {this.Name};";
//        return gameobjectPropLine;
//    }

//    public List<GameObjectProfile> GetGameObjectProfiles(GameObject go) // ici que la grosse recursion se passe
//    {
//        var children = GetChildren(go);

//        if (children.Count == 0)
//        {
//            return new List<GameObjectProfile>();
//        }
//        List<GameObjectProfile> childrenProfiles = new List<GameObjectProfile>();

//        foreach (var child in children)
//        {
//            childrenProfiles.Add(new GameObjectProfile(child));
//        }

//        return childrenProfiles;
//    }

//    public List<ComponentProfile> CreateComponentProfiles(GameObject go)
//    {
//        List<Component> components = go.GetComponents<Component>().ToList();
//        List<ComponentProfile> componentProfiles = new List<ComponentProfile>();
//        foreach (var component in components)
//        {
//            var profile = new ComponentProfile(component) { ParentName = go.name };
//            componentProfiles.Add(profile);
//        }
//        return componentProfiles;
//    }

//    private List<GameObject> GetChildren(GameObject go)
//    {
//        List<GameObject> children = new List<GameObject>();
//        foreach (Transform child in go.transform)
//        {
//            children.Add(child.gameObject);
//        }
//        return children;
//    }
//}

//public class ComponentProfile
//{
//    public Component Component;
//    public string TypeName;
//    public string ParentName;

//    private const string _publicPropertyPrefix = "public";

//    public ComponentProfile(Component component)
//    {
//        this.Component = component;
//        TypeName = component.GetType().Name;
//    }

//    public string GetComponentInitializeLine()
//    {
//        string componentInitLine = $"{ParentName}{TypeName} = {ParentName}.GetComponent<{TypeName}>();";
//        return componentInitLine;
//    }

//    public string GetComponentPropertyLine()
//    {
//        string componentPropertyLine = $"{_publicPropertyPrefix} {TypeName} {ParentName}{TypeName};";
//        return componentPropertyLine;
//    }
//}
