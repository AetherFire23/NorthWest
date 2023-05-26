using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
