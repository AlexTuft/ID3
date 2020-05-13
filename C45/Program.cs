using C45.Loaders;
using C45.Tree;
using C45.Tree.Drawing;
using System;
using System.Linq;

namespace C45
{
    public class Program
    {
        private const string FlagDrawTree = "--draw-tree";
        private const string FlagShowErrors = "--show-errors";

        private const string OptionSplitData = "--data-split"; // expects as next arg as 0.0 - 1.0

        private const string SwitchWeatherDataSet = "--dataset-weather";
        private const string SwitchZooDataSet = "--dataset-zoo";
        private const string SwitchTestMode = "--test"; // expects next arg as >=0

        private static string[] Args { get; set; }

        public static void Main(string[] args)
        {
            Args = args;

            int runs = GetRunCount();

            int totalAccuracy = 0;

            for (int i = 0; i < runs; i++)
            {
                DataSet dataSets = CreateDataSet();
                IDecisionTree tree = BuildTree(dataSets);

                DrawTree(args, tree);
                totalAccuracy += ValidateTree(args, dataSets, tree);
            }

            if (runs > 1)
            {
                Console.WriteLine($"Average accuracy {(double)totalAccuracy / runs}%");
            }
        }

        private static int GetRunCount()
        {
            var runCount = 1;
            if (Args.Contains(SwitchTestMode))
            {
                int.TryParse(Args[Array.IndexOf(Args, SwitchTestMode) + 1], out runCount);
            }
            return runCount;
        }

        private static DataSet CreateDataSet()
        {
            IDataFile dataFile = LoadData();
            var trainingTestSplitRatio = GetDataSplit();
            return new DataSet(dataFile, trainingTestSplitRatio);
        }

        private static double GetDataSplit()
        {
            var trainingTestSplitRatio = 0.7;
            if (Args.Contains(OptionSplitData))
            {
                double.TryParse(Args[Array.IndexOf(Args, OptionSplitData) + 1], out trainingTestSplitRatio);
            }

            return trainingTestSplitRatio;
        }

        private static IDataFile LoadData()
        {
            IDataFile dataFile = new IncomeDataFile();

            if (Args.Contains(SwitchZooDataSet))
            {
                dataFile = new ZooDataFile();
            }
            else if (Args.Contains(SwitchWeatherDataSet))
            {
                dataFile = new WeatherData();
            }

            return dataFile;
        }

        private static IDecisionTree BuildTree(DataSet dataSets)
        {
            var treeBuilder = new C45TreeBuilder();
            var tree = treeBuilder.BuildTree(dataSets.TrainingData, dataSets.TargetAttribute);
            return tree;
        }

        private static void DrawTree(string[] args, IDecisionTree tree)
        {
            if (args.Contains(FlagDrawTree))
            {
                Console.WriteLine(tree.Draw());
            }
        }

        private static int ValidateTree(string[] args, DataSet dataSets, IDecisionTree tree)
        {
            int totalPredictions = 0;
            int correctPredictions = 0;

            foreach (var row in dataSets.TestData.Rows())
            {
                try
                {
                    string predictedClassification = tree.Classify(row);

                    string @class = row[dataSets.TargetAttribute];
                    if (@class == predictedClassification)
                    {
                        correctPredictions++;
                    }
                }
                catch (ClassificationException ex)
                {
                    if (args.Contains(FlagShowErrors))
                    {
                        Console.WriteLine("Unable to classify row because: " + ex.Message);
                    }
                }

                totalPredictions++;
            }

            int accuracy = (int)((double)correctPredictions / totalPredictions * 100);
            Console.WriteLine($"Total predictions: {totalPredictions}, correct: {correctPredictions} ({accuracy}%)");

            return accuracy;
        }
    }
}
