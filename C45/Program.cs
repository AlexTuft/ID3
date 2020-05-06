using C45.Data;
using C45.Loaders;
using C45.Tree;
using C45.Tree.Drawing;
using System;

using static C45.Common.ListHelpers;

namespace C45
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataSets = ZooDatasetLoader.Load(0.7);
            var classificationAttributue = "class type";

            var treeBuilder = new C45TreeBuilder();
            var tree = treeBuilder.BuildTree(dataSets.TrainigData, classificationAttributue);
            
            Console.WriteLine(tree.Draw());

            int totalPredictions = 0;
            int correctPredictions = 0;

            foreach (var row in dataSets.TestData.Rows())
            {
                string @class = row[classificationAttributue];
                string predictedClassification = tree.Classify(row);

                if (@class == predictedClassification)
                {
                    correctPredictions++;
                }
                totalPredictions++;
            }

            Console.WriteLine($"Total predictions: {totalPredictions}, correct: {correctPredictions} ({(int)((double)correctPredictions / totalPredictions * 100)}%)");
        }
    }
}
