using System.Collections.Generic;

namespace C45.Common
{
    public static class ListHelpers
    {
        public static IList<T> ListOf<T>(params T[] items)
        {
            return new List<T>(items);
        }
    }
}
