using Assets;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

// assets must always be loaded before launching the game so
// -2 loading assets
// -1 initializing game
// [DefaultExecutionOrder(-2)]
namespace Assets.AssetLoading
{
    public class PrefabLoader : MonoBehaviour // should consider converting this to static so I can do PrefabLoader.Destroy()
    {
        // 1. Crate prefab
        // 2. drag prefab in editor to prefabAssetReferences
        // 3. Create a monobehaviour inheriting from PrefabBase
        // 4. Instantiate with await CreateInstanteOfAsync<MyUniqueScript>();
        // 5. OPTIONAL : create a .Initialize() method to act as a constructor that returns itself : await CreateInstanceOf<SquareCircle>().Initialize(string text)


        [SerializeField]
        private List<AssetReferenceGameObject> _prefabAssetReferences = new List<AssetReferenceGameObject>();
        private Dictionary<Type, AssetReference> _references = new();

        public async UniTask InitializeAsync()
        {
            await CreateAssetTypeMapAsync();
        }

        private async UniTask CreateAssetTypeMapAsync()
        {
            foreach (var assetReference in _prefabAssetReferences)
            {
                if (assetReference is null) continue;

                var asset = await assetReference.LoadAssetAsync();
                var componentType = asset.GetComponent<PrefabScriptBase>();
                if (componentType is null) throw new ArgumentException($"The following prefab {assetReference.Asset.name}  does not have any script that inherits from PrefabScriptBase!");

                _references.Add(componentType.GetType(), assetReference);
            }
        }

        // Do not forget that reflection is low
        // Might happen that I query some stuff and it is not spawened until finished loading
        public async UniTask<T> CreateInstanceOfAsync<T>(GameObject parent) where T : PrefabScriptBase
        {
            if(typeof(T) == typeof(YesNoDialog))
            {
                int i = 0;
            }


            if (!_references.ContainsKey(typeof(T))) throw new ArgumentNullException($"{typeof(T)} did not exist");

            AssetReference assetReference = _references[typeof(T)];
            GameObject gameObject = await assetReference.InstantiateAsync(parent.transform).Task.AsUniTask(); // lets try to convert this to a unitask
            T component = gameObject.GetComponent<T>();
            return component;
        }

        public void DestroyInstanceOf<T>(T instance) where T : PrefabScriptBase
        {
            var assetToDelete = _references.GetValueOrDefault(typeof(T));
            assetToDelete.ReleaseInstance(instance.gameObject);
        }
    }
}
