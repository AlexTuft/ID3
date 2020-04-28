using System.Collections.Generic;
using System.Linq;

namespace C45.Data
{
    public class ClassSummary
    {
        private readonly Dictionary<string, int> _classCounts = new Dictionary<string, int>();

        public ClassSummary(IDataTable data, string classAttribute)
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

        public bool AllClassesAreSame()
        {
            return Classes.Count() == 1;
        }
    }
}
