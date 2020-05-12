using C45.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class RemoveColumn : IDataFile
    {
        private readonly IDataFile _dataFile;
        private readonly string _columnName;
        private readonly int _columnNameIndex;

        public RemoveColumn(IDataFile dataFile, string columnName)
        {
            _dataFile = dataFile;
            _columnName = columnName;
            _columnNameIndex = _dataFile.Attributes.IndexOf(columnName);
        }

        public IList<string> Attributes => _dataFile.Attributes
            .Where(x => !string.Equals(x, _columnName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        public IEnumerable<IList<string>> Records => _dataFile.Records
            .Select(x =>
            {
                x = new List<string>(x);
                x.RemoveAt(_columnNameIndex);
                return x;
            });

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;
    }

    public static class RemoveColumnFluentExtension
    {
        public static IDataFile RemoveColumn(this IDataFile dataFile, string columnName)
        {
            return new RemoveColumn(dataFile, columnName);
        }
    }
}