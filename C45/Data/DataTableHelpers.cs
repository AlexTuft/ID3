using System.Collections.Generic;
using System.Linq;

namespace C45.Data
{
    public static class DataTableHelpers
    {
        public static ISet<string> GetUniqueValueForAttribute(this DataTable data, string attribute)
        {
            return data.Rows()
                .Select(x => x[attribute])
                .ToHashSet();
        }

        public static ISet<string> GetClassesForValue(this DataTable data, string attribute, string value, string classifier)
        {
            return data.Rows()
               .Where(x => x[attribute] == value)
               .Select(x => x[classifier])
               .ToHashSet();
        }

        public static int CountValueOccurrences(this DataTable data, string attribute, string value)
        {
            return data.Rows().Count(x => x[attribute] == value);
        }

        public static int CountClassOccurrences(this DataTable data, string attribute, string value, string classifier, string @class)
        {
            return data.Rows().Count(x => x[attribute] == value && x[classifier] == @class);
        }

        public static bool AllClassesAreSame(this DataTable data, string classifier)
        {
            return data.GetUniqueValueForAttribute(classifier).Count == 1;
        }
    }
}
