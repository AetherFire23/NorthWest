using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// assets must always be loaded before launching the game so
// -2 loading assets
// -1 initializing game
// [DefaultExecutionOrder(-2)]
namespace Assets.AssetLoading
{
    public class PrefabLoader : MonoBehaviour // should consider exposing at least some static methods so that PrefabLoader.Destroy() works
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

        // Do not forget that reflection for typeof and nameof is quite fast for reflection
        public async UniTask<T> CreateInstanceOfAsync<T>(GameObject parent) where T : PrefabScriptBase
        {
            if (!_references.ContainsKey(typeof(T))) throw new ArgumentNullException($"{typeof(T)} did not exist");

            AssetReference assetReference = _references[typeof(T)];
            GameObject gameObject = await assetReference.InstantiateAsync(parent.transform).Task.AsUniTask();
            T component = gameObject.GetComponent<T>();
            return component;
        }

        public void DestroyInstanceOf<T>(T instance) where T : PrefabScriptBase
        {
            var assetToDelete = _references.GetValueOrDefault(typeof(T));
            assetToDelete.ReleaseInstance(instance.gameObject);
        }

        public T CreateInstanceOf<T>(GameObject parent) where T : PrefabScriptBase
        {
            AssetReference assetReference = _references[typeof(T)];
            GameObject gameObject = assetReference.InstantiateAsync().Task.Result;
            T component = gameObject.GetComponent<T>();
            return component;
        }

        public async UniTask<T> CreateInstanceOfAsync<T>(MonoBehaviour behaviour) where T : PrefabScriptBase
        {
            var instance = await this.CreateInstanceOfAsync<T>(behaviour.gameObject);
            return instance;
        }
    }
}
