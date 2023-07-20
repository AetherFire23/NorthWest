using Shared_Resources.Extensions;
using System.Collections.Generic;
using System.Linq;
using Shared_Resources.Interfaces;
using System.Text;
namespace Assets.EntityRefresh
{
    public static class EntityComparer
    {
        public static ComparedEntities<TPrefab, TEntity> CompareEntities<TPrefab, TEntity>(List<TPrefab> currentPrefabs, List<TEntity> upToDateEntities)
            where TEntity : IEntity
            where TPrefab : PrefabScriptBase, IEntity
        {
            var appearedEntities = upToDateEntities.Where(e => !currentPrefabs.ContainsEntity(e)).ToList();

            // still not tested if this works !
            var disappearedPrefabs = currentPrefabs.Where(e => !upToDateEntities.ContainsEntity(e)).ToList();

            var compared = new ComparedEntities<TPrefab, TEntity>()
            {
                AppearedEntities = appearedEntities,
                DisappearedPrefabs = disappearedPrefabs,
            };

            
            return compared;
        }

        //public static ComparedEntities<TPrefab, TEntity>
    }
}
