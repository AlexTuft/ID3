using C45.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace C45.Loaders
{
    public static class ZooDatasetLoader
    {
        private const int ClassFileClassNumberColumnIndex = 0;
        private const int ClassFileClassLabelColumnIndex = 2;

        public static (DataTable TrainigData, DataTable TestData) Load(double trainingTestSplitRatio)
        {
            var attributesAndRecords = LoadAttributesAndRecords();
            var trainingAndTestData = SplitData(attributesAndRecords.Records, trainingTestSplitRatio);

            var trainingSet = new DataTable(attributesAndRecords.Attributes);
            trainingSet.AddRows(trainingAndTestData.TrainingData);

            var testSet = new DataTable(attributesAndRecords.Attributes);
            testSet.AddRows(trainingAndTestData.TestData);

            return (trainingSet, testSet);
        }

        private static (IList<IList<string>> TrainingData, IList<IList<string>> TestData) SplitData(
            IEnumerable<IList<string>> records, double trainingTestSplitRatio)
        {
            var shuffledRecords = records.OrderBy(_ => Guid.NewGuid())
                .ToList();

            var splitPoint = (int)(shuffledRecords.Count * trainingTestSplitRatio);

            return (shuffledRecords.Take(splitPoint).ToList(),
                shuffledRecords.Skip(splitPoint).ToList());
        }

        private static (IList<string> Attributes, IEnumerable<IList<string>> Records) LoadAttributesAndRecords()
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
