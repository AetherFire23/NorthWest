using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Properties;

namespace Assets.UtilsAssembly
{
    public static class CSExtensions
    {
        public static List<T> GetEnumValues<T>() where T : Enum
        {
            var allEnumTypes = (T[])Enum.GetValues(typeof(T));
            return allEnumTypes.ToList();
        }

        public static bool IsEmpty<T>(List<T> list)
        {
            return !list.Any();
        }

        //public static bool IsAssignableFrom<T>(this object obj)
        //{
        //    bool isInherited = obj.GetType().IsAssignableFrom(typeof(T));
        //    return isInherited;
        //}
    }
}
