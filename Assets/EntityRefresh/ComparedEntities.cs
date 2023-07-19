using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EntityRefresh
{
    public class ComparedEntities<TPrefab, TEntity>
        where TPrefab : PrefabScriptBase, IEntity
        where TEntity : IEntity
    {
        public List<TEntity> AppearedEntities;
        public List<TPrefab> DisappearedPrefabs;
    }
}
