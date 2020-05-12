using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class ShuffleRecords : IDataFile
    {
        private readonly IDataFile _dataFile;

        public ShuffleRecords(IDataFile dataFile)
        {
            _dataFile = dataFile;
        }

        public IEnumerable<string> Attributes => _dataFile.Attributes;

        public IEnumerable<IList<string>> Records => _dataFile.Records.OrderBy(x => Guid.NewGuid());

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;
    }

    public static class ShuffleRecordsFluentExtension
    {
        public static IDataFile ShuffleRecords(this IDataFile dataFile)
        {
            return new ShuffleRecords(dataFile);
        }
    }
}
