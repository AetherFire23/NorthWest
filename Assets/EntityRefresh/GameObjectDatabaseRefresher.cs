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
        private Dictionary<TPrefab, TEntity> GameObjectsAndEntities = new Dictionary<TPrefab, TEntity>();
        private List<Guid> _gameObjectIds => GameObjectsAndEntities.Keys.Select(x => x.Id).ToList();
        private List<TPrefab> _gameObjects => GameObjectsAndEntities.Keys.ToList();

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

        public GameObjectDatabaseRefresher(Func<TEntity, UniTask<TPrefab>> createGameObjectFromEntity, Action<TPrefab> destroyFunction, bool deleteMissing = true)
        {
            if (createGameObjectFromEntity is null)
            {
                Debug.LogError($"Cannot leave{createGameObjectFromEntity} null in entity refresh.");
                throw new Exception();
            }

            _createGameObjectFromEntity = createGameObjectFromEntity;
            _destroyFunction = destroyFunction;
            _deleteMissing = deleteMissing;
        }

        public async UniTask RefreshEntities(List<TEntity> entities)
        {
            try
            {
                var existingEntitiesids = GameObjectsAndEntities.Keys.Select(x => x.Id);

                foreach (TEntity entity in await GetAppearedEntities(entities))
                {
                    var newGameObject = await _createGameObjectFromEntity(entity);
                    GameObjectsAndEntities.Add(newGameObject, entity);
                }

                if (_deleteMissing) // cest que 
                {
                    foreach (TPrefab missingEntity in await GetDisappeardEntities(entities))
                    {
                        _destroyFunction(missingEntity);
                        GameObjectsAndEntities.Remove(missingEntity);
                    }
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
