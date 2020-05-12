using System.Collections.Generic;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class NormalizeRecords : IDataFile
    {
        private readonly IDataFile _dataFile;

        public NormalizeRecords(IDataFile dataFile)
        {
            _dataFile = dataFile;
        }

        public IList<string> Attributes => _dataFile.Attributes;

        public IEnumerable<IList<string>> Records => _dataFile.Records
            .Select(x => x.Select(y => Normalize(y)).ToList() as IList<string>);

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;

        private string Normalize(string x)
        {
            return x.Trim().ToLowerInvariant();
        }
    }

    public static class NormalizeRecordsFluentExtension
    {
        public static IDataFile NormalizeRecords(this IDataFile dataFile)
        {
            return new NormalizeRecords(dataFile);
        }
    }
}
