using C45.Data;
using System.Collections.Generic;
using System.Linq;

namespace ID3.Tree
{
    /// <summary>
    /// Contains extension methods for DataTable to help build the tree and calculate entropy/information gain
    /// </summary>
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

        public static string GetMostCommonOutcome(this DataTable data, string targetAttribute)
        {
            return data.Rows()
                .GroupBy(x => x[targetAttribute])
                .OrderByDescending(x => x.Key)
                .Select(x => x.First()[targetAttribute])
                .First();
        }
    }
}
