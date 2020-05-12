using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C45.Loaders.DataFunctions
{
    public class FilterRecords : IDataFile
    {
        private readonly IDataFile _dataFile;
        private readonly int _columnNameIndex;
        private readonly Func<string, bool> _predicate;

        public FilterRecords(IDataFile dataFile, string columnName, Func<string, bool> predicate)
        {
            _dataFile = dataFile;
            _columnNameIndex = _dataFile.Attributes.IndexOf(columnName);
            _predicate = predicate;
        }

        public IList<string> Attributes => _dataFile.Attributes;

        public IEnumerable<IList<string>> Records => _dataFile.Records
            .Where(x => _predicate(x[_columnNameIndex]));

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;
    }

    public static class FilterRecordsFluentExtension
    {
        public static IDataFile FilterRecords(this IDataFile dataFile, string columnName, Func<string, bool> predicate)
        {
            return new FilterRecords(dataFile, columnName, predicate);
        }
    }
}
