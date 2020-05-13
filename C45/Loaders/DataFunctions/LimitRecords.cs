using System.Collections.Generic;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class LimitRecords : IDataFile
    {
        private readonly IDataFile _dataFile;
        private readonly int _count;

        public LimitRecords(IDataFile dataFile, int count)
        {
            _dataFile = dataFile;
            _count = count;
        }

        public IEnumerable<string> Attributes => _dataFile.Attributes;

        public IEnumerable<IList<string>> Records => _dataFile.Records.Take(_count);

        public string TargetAttribute => _dataFile.TargetAttribute;
    }

    public static class LimitRecordsFluentExtension
    {
        public static IDataFile LimitRecords(this IDataFile dataFile, int count)
        {
            return new LimitRecords(dataFile, count);
        }
    }
}
