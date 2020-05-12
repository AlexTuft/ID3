using C45.Data;
using C45.Loaders;
using C45.Tree;
using C45.Tree.Drawing;
using System;


namespace C45
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataSets = new DataSet(new ZooDataFile(), trainingTestSplitRatio: 0.7);

            var treeBuilder = new C45TreeBuilder();
            var tree = treeBuilder.BuildTree(dataSets.TrainingData, dataSets.ClassificationAttribute);
            
            Console.WriteLine(tree.Draw());

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
                    Console.WriteLine("Unable to classify row because: " + ex.Message);
                }

                totalPredictions++;
            }

            Console.WriteLine($"Total predictions: {totalPredictions}, correct: {correctPredictions} ({(int)((double)correctPredictions / totalPredictions * 100)}%)");
        }
    }
}
