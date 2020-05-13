using C45.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace C45.Loaders
{
    public class DataSet
    {
        public DataSet(IDataFile dataFile,
            double trainingTestSplitRatio,
            bool dumpData)
        {
            var dataSets = Load(dataFile, trainingTestSplitRatio);
            TrainingData = dataSets.TrainingData;
            TestData = dataSets.TestData;
            TargetAttribute = dataFile.TargetAttribute;

            if (dumpData)
            {
                DumpData(TrainingData, TestData);
            }
        }

        public DataTable TrainingData { get; }

        public DataTable TestData { get; }

        public string TargetAttribute { get; }

        private static (DataTable TrainingData, DataTable TestData) Load(
            IDataFile dataFileLoader,
            double trainingTestSplitRatio)
        {
            var trainingAndTestData = SplitData(dataFileLoader.Records, trainingTestSplitRatio);

            var attributes = dataFileLoader.Attributes.ToList();
            var trainingSet = new DataTable(attributes);
            trainingSet.AddRows(trainingAndTestData.TrainingData);

            var testSet = new DataTable(attributes);
            testSet.AddRows(trainingAndTestData.TestData);

            return (trainingSet, testSet);
        }

        private static void DumpData(DataTable trainingSet, DataTable testSet)
        {
            var setId = Guid.NewGuid();
            if (!Directory.Exists("DataDump"))
            {
                Directory.CreateDirectory("DataDump");
            }
            trainingSet.Dump($"DataDump/{setId}_training.csv");
            testSet.Dump($"DataDump/{setId}_test.csv");
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
    }
}
