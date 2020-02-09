using C45.Data;
using System.Collections.Generic;

namespace C45.Tree
{
    public class ValueClassCounts
    {
        private readonly Dictionary<string, int> _valueCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, Dictionary<string, int>> _valueClassCounts = new Dictionary<string, Dictionary<string, int>>();
            
        public ValueClassCounts(IDataTable data, string attribute, string classAttribute)
        {
            foreach (var row in data.Rows())
            {
                var value = row[attribute];
                if (!_valueCounts.ContainsKey(value))
                {
                    _valueCounts[value] = 1;
                    _valueClassCounts[value] = new Dictionary<string, int>();
                }
                else
                {
                    _valueCounts[value]++;
                }

                var @class = row[classAttribute];
                if (!_valueClassCounts[value].ContainsKey(@class))
                {
                    _valueClassCounts[value][@class] = 1;
                }
                else
                {
                    _valueClassCounts[value][@class]++;
                }
            }
        }

        public IEnumerable<string> UniqueValues => _valueCounts.Keys;

        public int CountOfValue(string value)
        {
            return _valueCounts[value];
        }

        public IEnumerable<string> ClassesFor(string value)
        {
            return _valueClassCounts[value].Keys;
        }

        public int CountOfClass(string value, string @class)
        {
            return _valueClassCounts[value][@class];
        }
    }
}
