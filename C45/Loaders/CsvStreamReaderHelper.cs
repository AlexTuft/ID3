using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace C45.Loaders
{
    public static class CsvStreamReaderHelper
    {
        private const char Delimiter = ',';

        public static IEnumerable<string> CSVReadLine(this StreamReader reader)
        {
            return reader.ReadLine().Split(Delimiter);
        }

        public static IEnumerable<IList<string>> CSVLines(this StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                yield return reader.CSVReadLine().ToList();
            }
        }
    }
}
