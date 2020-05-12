using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class FilterRecords : IDataFile
    {
        private readonly IDataFile _dataFile;
        private readonly Func<IList<string>, bool> _predicate;

        public FilterRecords(IDataFile dataFile, Func<IList<string>, bool> predicate)
        {
            _dataFile = dataFile;
            _predicate = predicate;
        }

        public IEnumerable<string> Attributes => _dataFile.Attributes;

        public IEnumerable<IList<string>> Records => _dataFile.Records
            .Where(x => _predicate(x));

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;
    }

    public static class FilterRecordsFluentExtension
    {
        public static IDataFile FilterRecords(this IDataFile dataFile, Func<IList<string>, bool> predicate)
        {
            return new FilterRecords(dataFile, predicate);
        }
    }
}
