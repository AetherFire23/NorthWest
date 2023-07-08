using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utils
{
    public class EntityRefreshResult<T> where T : IEntity
    {
        // juste renvoyer lui qui a change ?
        public List<T> Modified = new List<T>();
        public List<T> Added = new List<T>();

        public static EntityRefreshResult<T> GetModifiedOrCreated(List<T> stored, List<T> fresh)
        {
            var storedGuids = stored.Select(x => x.Id).ToList();

            var result = new EntityRefreshResult<T>();

            result.Modified = fresh.Where(x => storedGuids.Contains(x.Id)).ToList();
            result.Added = fresh.Where(x=> !storedGuids.Contains(x.Id)).ToList();
            return result;
        }
    }
}
