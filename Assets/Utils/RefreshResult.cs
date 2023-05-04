using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utils
{
    public class RefreshResult<TPrefab, TEntity> 
        where TPrefab : PrefabScriptBase, IEntity
        where TEntity : IEntity
    {
        public List<TEntity> Appeared;
        public List<TPrefab> Disappeared;
        public static RefreshResult<TPrefab, TEntity> GetRefreshResult(IEnumerable<TPrefab> oldEntities, IEnumerable<TEntity> upToDateEntities)
        {
            IEnumerable<TEntity> appeared = UnityExtensions.GetAppearedEntities(oldEntities, upToDateEntities);
            IEnumerable<TPrefab> disappeared = UnityExtensions.GetDisappearedGameObjects(oldEntities, upToDateEntities);
            var result = new RefreshResult<TPrefab, TEntity>()
            {
                Appeared = appeared.ToList(),
                Disappeared = disappeared.ToList(),
            };
            return result;
        }
    }
}
