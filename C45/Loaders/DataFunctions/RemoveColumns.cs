using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class RemoveColumns : IDataFile
    {
        private readonly IDataFile _dataFile;
        private readonly IList<string> _namesOfRemovedColumns;
        private readonly ISet<int> _indexesOfRemovedColumns;

        public RemoveColumns(IDataFile dataFile, IList<string> columnNames)
        {
            _dataFile = dataFile;
            _namesOfRemovedColumns = columnNames;
            _indexesOfRemovedColumns = new HashSet<int>();

            var attributes = dataFile.Attributes.ToList();
            foreach (var columnName in columnNames)
            {
                _indexesOfRemovedColumns.Add(attributes.IndexOf(columnName));
            }
        }

        public IEnumerable<string> Attributes => _dataFile.Attributes
            .Except(_namesOfRemovedColumns);

        public IEnumerable<IList<string>> Records => _dataFile.Records
            .Select(x =>
            {

                var y = new List<string>();
                for (int i = 0; i < x.Count; i++)
                {
                    if (!_indexesOfRemovedColumns.Contains(i))
                    {
                        y.Add(x[i]);
                    }
                }
                return y;
            });

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;
    }

    public static class RemoveColumnFluentExtension
    {
        public static IDataFile RemoveColumns(this IDataFile dataFile, params string[] columnNames)
        {
            return new RemoveColumns(dataFile, columnNames.ToList());
        }
    }
}