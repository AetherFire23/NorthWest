using Cysharp.Threading.Tasks;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    public class GameObjectDatabaseRefresher<TPrefab, TEntity>
        where TPrefab : PrefabScriptBase, IEntity
        where TEntity : IEntity// Avec du inheritance problablmement genre que UpdateEntities cest virtual au pire cest dans les tick jsp  
    {
        //public Dictionary<TPrefab, TEntity> GameObjectsAndEntities = new Dictionary<TPrefab, TEntity>();

        public List<Tuple<TPrefab, TEntity>> GameObjectsAndEntities = new List<Tuple<TPrefab, TEntity>>();

        private List<Guid> _gameObjectIds => GameObjectsAndEntities.Select(x => x.Item1.Id).ToList();
        private List<TPrefab> _gameObjects => GameObjectsAndEntities.Select(x => x.Item1).ToList();


        private Func<TEntity, UniTask<TPrefab>> _createGameObjectFromEntity;
        private Action<TPrefab> _destroyFunction;
        private bool _deleteMissing;
        private bool _initializableByEntity;

        public GameObjectDatabaseRefresher(Func<TEntity, UniTask<TPrefab>> createGameObjectFromEntity, bool deleteMissing = true)
        {
            if (createGameObjectFromEntity is null)
            {
                Debug.LogError($"Cannot leave{createGameObjectFromEntity} null in entity refresh.");
                throw new Exception();
            }
            _createGameObjectFromEntity = createGameObjectFromEntity;
            _destroyFunction = DefaultDestroy;
            _deleteMissing = deleteMissing;

        }

        public GameObjectDatabaseRefresher(Func<TEntity, UniTask<TPrefab>> createGameObjectFromEntity, Action<TPrefab> customDestroy, bool deleteMissing = true)
        {
            if (createGameObjectFromEntity is null)
            {
                Debug.LogError($"Cannot leave{createGameObjectFromEntity} null in entity refresh.");
                throw new Exception();
            }

            _createGameObjectFromEntity = createGameObjectFromEntity;
            _destroyFunction = customDestroy;
            _deleteMissing = deleteMissing;
        }

        public async UniTask RefreshEntities(List<TEntity> uptoDate)
        {
            try
            {
                var existingEntitiesids = GameObjectsAndEntities.Select(x => x.Item1.Id);

                var appeared = await GetAppearedEntities(uptoDate);
                foreach (TEntity entity in appeared)
                {
                    var newGameObject = await _createGameObjectFromEntity(entity);
                    var t = new Tuple<TPrefab, TEntity>(newGameObject, entity);
                    GameObjectsAndEntities.Add(t);
                }

                if (_deleteMissing) // cest que 
                {
                    foreach (TPrefab missingEntity in await GetDisappeardEntities(uptoDate))
                    {
                        _destroyFunction(missingEntity);
                        Tuple<TPrefab, TEntity> found = GameObjectsAndEntities.First(x => x.Item1.Id == missingEntity.Id);
                        GameObjectsAndEntities.Remove(found);
                    }
                }

                // update stuff

                foreach (var old in GameObjectsAndEntities)
                {
                    var newCorrespondingToOld = uptoDate.First(x => old.Item1.Id == x.Id);
                    var oldEntity = GameObjectsAndEntities.FirstOrDefault(x => x.Item2.Id == old.Item2.Id);
                    var s = oldEntity.Item2;
                    s = newCorrespondingToOld;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        public async UniTask<List<TEntity>> GetAppearedEntities(List<TEntity> entities)
        {
            var appeared = entities.Where(x => !_gameObjectIds.Contains(x.Id));
            return appeared.ToList();
        }

        public async UniTask<List<TPrefab>> GetDisappeardEntities(List<TEntity> entities)
        {
            var entityIds = entities.Select(x => x.Id).ToList();
            var disappeared = _gameObjects.Where(x => !entityIds.Contains(x.Id));
            return disappeared.ToList();
        }

        public void DefaultDestroy(TPrefab entity)
        {
            GameObject.Destroy(entity.transform.gameObject);
        }
    }
}
