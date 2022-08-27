using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class InstanceWrapper<T> // where T == the script on the main gameobject
{
    public GameObject UnityInstance;
    public T InstanceBehaviour;

    private GameObject _prefab;
    private string _resourceName;
    private string _parentName;

    /// <summary>
    /// This constructor instantiates the unity instance under the given gameobject 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public InstanceWrapper(string resourceName, GameObject parent)
    {
        _resourceName = resourceName;
        _prefab = LoadPrefab();
        UnityInstance = CreateInstanceUnderParent(_prefab, parent);
        InstanceBehaviour = UnityInstance.GetComponentSafely<T>();
    }

    private GameObject LoadPrefab()
    {
        return UnityExtensions.LoadPrefabSafely(_resourceName) as GameObject;
    }

    private GameObject CreateInstanceUnderParent(GameObject toInstantiate, GameObject parent)
    {
        var instantiatedPrefab = GameObject.Instantiate(toInstantiate, parent.transform);
        return instantiatedPrefab;
    }
}