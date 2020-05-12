using System.IO;

namespace C45.Loaders
{
    public static class DataDumpExtensions
    {
        public static void Dump(this IDataFile dataFile, string fileName)
        {
            using var file = new StreamWriter(new FileStream(fileName, FileMode.Create));

            file.WriteLine(string.Join(',', dataFile.Attributes));
            foreach (var record in dataFile.Records)
            {
                file.WriteLine(string.Join(',', record));
            }

            file.Flush();
        }
    }
}
