using C45.Data;
using System.Collections.Generic;

namespace C45.Tree
{
    public class ClassCounts
    {
        private readonly Dictionary<string, int> _classCounts = new Dictionary<string, int>();

        public ClassCounts(IDataTable data, string classAttribute)
        {
            foreach (var row in data.Rows())
            {
                var @class = row[classAttribute];
                if (!_classCounts.ContainsKey(@class))
                {
                    _classCounts[@class] = 1;
                }
                else
                {
                    _classCounts[@class]++;
                }
            }
        }

        public IEnumerable<string> Classes => _classCounts.Keys;

        public int CountOf(string @class)
        {
            return _classCounts[@class];
        }
    }
}
