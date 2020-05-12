using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class GroupColumn : IDataFile
    {
        private readonly IDataFile _dataFile;
        private readonly int _columnNameIndex;
        private readonly Func<string, string> _groupBy;

        public GroupColumn(IDataFile dataFile, string columnName, Func<string, string> groupBy)
        {
            _dataFile = dataFile;
            _columnNameIndex = Attributes.ToList().IndexOf(columnName);
            _groupBy = groupBy;
        }

        public IEnumerable<string> Attributes => _dataFile.Attributes;

        public IEnumerable<IList<string>> Records => _dataFile.Records
            .Select(x =>
            {
                x[_columnNameIndex] = _groupBy(x[_columnNameIndex]);
                return x;
            });

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;
    }

    public static class GroupColumnFluentExtension
    {
        public static IDataFile GroupColumn(this IDataFile dataFile, string columnName, Func<string, string> groupBy)
        {
            return new GroupColumn(dataFile, columnName, groupBy);
        }
    }
}
