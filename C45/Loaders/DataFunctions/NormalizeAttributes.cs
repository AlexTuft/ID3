using System.Collections.Generic;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class NormalizeAttributes : IDataFile
    {
        private readonly IDataFile _dataFile;

        public NormalizeAttributes(IDataFile dataFile)
        {
            _dataFile = dataFile;
        }

        public IList<string> Attributes => _dataFile.Attributes
            .Select(x => Normalize(x))
            .ToList();

        public IEnumerable<IList<string>> Records => _dataFile.Records;

        public string ClassificationAttribute => Normalize(_dataFile.ClassificationAttribute);

        private string Normalize(string x)
        {
            return x.Trim().ToLowerInvariant();
        }
    }

    public static class NormalizeAttributesFluentExtension
    {
        public static IDataFile NormalizeAttributes(this IDataFile dataFile)
        {
            return new NormalizeAttributes(dataFile);
        }
    }
}
