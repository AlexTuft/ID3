using C45.Data;
using System.IO;

namespace C45.Loaders
{
    public static class DataDumpExtensions
    {
        public static void Dump(this DataTable data, string fileName)
        {
            using var file = new StreamWriter(new FileStream(fileName, FileMode.Create));

            file.WriteLine(string.Join(',', data.Attributes));
            foreach (var row in data.Rows())
            {
                file.WriteLine(string.Join(',', row));
            }

            file.Flush();
        }
    }
}
