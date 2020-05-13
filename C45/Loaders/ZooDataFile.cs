using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace C45.Loaders
{
    public class ZooDataFile : IDataFile
    {
        private const int ClassFileClassNumberColumnIndex = 0;
        private const int ClassFileClassLabelColumnIndex = 2;

        public ZooDataFile()
        {
            var attributesAndRecords = GetAttributesAndRecords();
            Attributes = attributesAndRecords.Attributes;
            Records = attributesAndRecords.Records;
        }

        public IEnumerable<string> Attributes { get; }

        public IEnumerable<IList<string>> Records { get; }

        public string TargetAttribute => "class type";

        private static (IList<string> Attributes, IEnumerable<IList<string>> Records) GetAttributesAndRecords()
        {
            using var file = new StreamReader(new FileStream("Resources/zoo.csv", FileMode.Open));
            return (ReadAttributes(file), ReadRecords(file));
        }

        private static IList<string> ReadAttributes(StreamReader file) =>
            file.CSVReadLine()
            .Skip(1) // Skip animal name
            .Select(x => x.Replace('_', ' '))
            .ToList();

        private static IList<IList<string>> ReadRecords(StreamReader file)
        {
            var classifications = LoadClassifications();

            return file.CSVLines()
                .Select(x => new List<string>
                {
                    // Skip animal name
                    x[1] == "1" ? "yes" : "no",
                    x[2] == "1" ? "yes" : "no",
                    x[3] == "1" ? "yes" : "no",
                    x[4] == "1" ? "yes" : "no",
                    x[5] == "1" ? "yes" : "no",
                    x[6] == "1" ? "yes" : "no",
                    x[7] == "1" ? "yes" : "no",
                    x[8] == "1" ? "yes" : "no",
                    x[9] == "1" ? "yes" : "no",
                    x[10] == "1" ? "yes" : "no",
                    x[11] == "1" ? "yes" : "no",
                    x[12] == "1" ? "yes" : "no",
                    x[13],
                    x[14] == "1" ? "yes" : "no",
                    x[15] == "1" ? "yes" : "no",
                    x[16] == "1" ? "yes" : "no",
                    classifications[x[17]],
                } as IList<string>)
                .ToList();
        }

        private static IDictionary<string, string> LoadClassifications()
        {
            var file = new StreamReader(new FileStream("Resources/class.csv", FileMode.Open));

            file.ReadLine(); // discard headers

            return file.CSVLines()
                .ToDictionary(x => x[ClassFileClassNumberColumnIndex],
                    x => x[ClassFileClassLabelColumnIndex]);
        }
    }
}
