using System.Collections.Generic;
using System.Linq;

namespace C45.Data
{
    public static class DataTableHelpers
    {
        public static ISet<string> GetDistinctValuesForAttribute(this DataTable data, string attribute)
        {
            return data.Rows()
                .Select(x => x[attribute])
                .ToHashSet();
        }

        public static ISet<string> GetDistinctOutcomes(this DataTable data, string attribute, string value, string targetAttribute)
        {
            return data.Rows()
               .Where(x => x[attribute] == value)
               .Select(x => x[targetAttribute])
               .ToHashSet();
        }

        public static int CountValueOccurrences(this DataTable data, string attribute, string value)
        {
            return data.Rows().Count(x => x[attribute] == value);
        }

        public static int CountOutcomeOccurrences(this DataTable data, string attribute, string value, string targetAttribute, string @class)
        {
            return data.Rows().Count(x => x[attribute] == value && x[targetAttribute] == @class);
        }

        public static bool AllClassesAreSame(this DataTable data, string targetAttribute)
        {
            return data.GetDistinctValuesForAttribute(targetAttribute).Count == 1;
        }
    }
}
