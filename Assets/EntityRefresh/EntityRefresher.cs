using Assets.Utils;
using Cysharp.Threading.Tasks;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EntityRefresh
{
    public class EntityRefresher<TPrefab, TEntity>
        where TPrefab : PrefabScriptBase, IEntity
        where TEntity : IEntity
    { 
        public List<TPrefab> Instances { get; set; } = new List<TPrefab>();
        private Func<TEntity, UniTask<TPrefab>> _createPrefab { get; set; }
        private Func<TPrefab, UniTask> _deletePrefab { get; set; }

        public EntityRefresher(Func<TEntity, UniTask<TPrefab>> createPrefabFunc, Func<TPrefab, UniTask> deletePrefabFunc = null)
        {

            _createPrefab = createPrefabFunc;
            _deletePrefab = deletePrefabFunc;
        }

        public async UniTask RefreshEntities(List<TEntity> upToDateEntities)
        {
            var refreshResult = EntityComparer.CompareEntities(Instances, upToDateEntities);
            foreach (TEntity entity in refreshResult.AppearedEntities)
            {
                var createdPrefab = await _createPrefab.Invoke(entity);
                Instances.Add(createdPrefab);
            }
            foreach (TPrefab prefab in refreshResult.DisappearedPrefabs)
            {
                if (_deletePrefab is null)
                {
                    Instances.Remove(prefab);

                    GameObject.Destroy(prefab.gameObject);
                }
                else
                {
                    Instances.Remove(prefab);

                    await _deletePrefab.Invoke(prefab);
                }
            }
        }
    }
}
