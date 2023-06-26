using Shared_Resources.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Utils
{
    /// <summary>
    /// Helper class to decouple the identification of the differences between server and client from the actual games logic
    /// </summary>
    public class RefreshResult<TPrefab, TEntity>
        where TPrefab : PrefabScriptBase, IEntity
        where TEntity : IEntity
    {
        public List<TEntity> AppearedEntities;
        public List<TPrefab> DisappearedPrefabs;

        /// <summary>
        /// Based on a Guid Identifier,Will return 1) appeared ENTITIES and 2) disappeared PREFABS from the GameState
        /// </summary>
        public static RefreshResult<TPrefab, TEntity> GetRefreshResult(IEnumerable<TPrefab> oldEntities, IEnumerable<TEntity> upToDateEntities)
        {
            // should move appeared-disappeard in here 
            IEnumerable<TEntity> appeared = UnityExtensions.GetAppearedEntities(oldEntities, upToDateEntities);
            IEnumerable<TPrefab> disappeared = UnityExtensions.GetDisappearedGameObjects(oldEntities, upToDateEntities);
            var result = new RefreshResult<TPrefab, TEntity>()
            {
                AppearedEntities = appeared.ToList(),
                DisappearedPrefabs = disappeared.ToList(),
            };
            return result;
        }
    }
}
