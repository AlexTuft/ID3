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

        private const string Option_SplitData = "--data-split"; // expects as next arg

        private const string SwitchWeatherDataSet = "--dataset-weather";
        private const string SwitchZooDataSet = "--dataset-zoo";

        private static string[] Args { get; set; }

        public static void Main(string[] args)
        {
            Args = args;

            DataSet dataSets = CreateDataSet();
            IDecisionTree tree = BuildTree(dataSets);

            DrawTree(args, tree);
            ValidateTree(args, dataSets, tree);
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
            if (Args.Contains(Option_SplitData))
            {
                double.TryParse(Args[Array.IndexOf(Args, Option_SplitData) + 1], out trainingTestSplitRatio);
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
            var tree = treeBuilder.BuildTree(dataSets.TrainingData, dataSets.ClassificationAttribute);
            return tree;
        }

        private static void DrawTree(string[] args, IDecisionTree tree)
        {
            if (args.Contains(FlagDrawTree))
            {
                Console.WriteLine(tree.Draw());
            }
        }

        private static void ValidateTree(string[] args, DataSet dataSets, IDecisionTree tree)
        {
            int totalPredictions = 0;
            int correctPredictions = 0;

            foreach (var row in dataSets.TestData.Rows())
            {
                try
                {
                    string predictedClassification = tree.Classify(row);

                    string @class = row[dataSets.ClassificationAttribute];
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

            Console.WriteLine($"Total predictions: {totalPredictions}, correct: {correctPredictions} ({(int)((double)correctPredictions / totalPredictions * 100)}%)");
        }
    }
}
