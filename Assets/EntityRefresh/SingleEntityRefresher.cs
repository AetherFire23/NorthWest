using Assets.AssetLoading;
using Assets.Utils;
using Cysharp.Threading.Tasks;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scratch
{
    public class SingleEntityRefresher<TPrefab, TEntity>
        where TPrefab : PrefabScriptBase, IEntity
        where TEntity : IEntity
    {
        public List<TPrefab> Instances { get; set; } = new List<TPrefab>();
        private Func<TEntity, UniTask<TPrefab>> _createPrefab { get; set; }
        private Func<TPrefab, UniTask> _deletePrefab { get; set; }

        public SingleEntityRefresher(Func<TEntity, UniTask<TPrefab>> createPrefab, Func<TPrefab, UniTask> deletePrefab = null)
        {
            _createPrefab = createPrefab;
            _deletePrefab = deletePrefab;
        }

        public async UniTask RefreshEntites(List<TEntity> upToDateEntities)
        {
            var refreshResult = RefreshResult<TPrefab, TEntity>.GetRefreshResult(Instances, upToDateEntities);
            foreach (TEntity entity in refreshResult.AppearedEntities)
            {
                var createdPrefab = await _createPrefab.Invoke(entity);
                Instances.Add(createdPrefab);
            }
            foreach (TPrefab prefab in refreshResult.DisappearedPrefabs)
            {
                if (_deletePrefab is null)
                {
                    GameObject.Destroy(prefab);
                }
                else
                {
                    await _deletePrefab.Invoke(prefab);
                }
            }
        }
    }
}
