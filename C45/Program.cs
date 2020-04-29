using C45.Data;
using C45.Tree;
using System;

using static C45.Common.ListHelpers;

namespace C45
{
    class Program
    {
        static void Main(string[] args)
        {
            var trainingData = new DataTable(ListOf("Outlook", "Temprature", "Humidity", "Windy", "Play"));
            trainingData.AddRow(ListOf("sunny", "hot", "high", "false", "no"));
            trainingData.AddRow(ListOf("sunny", "hot", "high", "true", "no"));
            trainingData.AddRow(ListOf("overcast", "hot", "high", "false", "yes"));
            trainingData.AddRow(ListOf("rainy", "mild", "high", "false", "yes"));
            trainingData.AddRow(ListOf("rainy", "cool", "normal", "false", "yes"));
            trainingData.AddRow(ListOf("rainy", "cool", "normal", "true", "no"));
            trainingData.AddRow(ListOf("overcast", "cool", "normal", "true", "yes"));
            trainingData.AddRow(ListOf("sunny", "mild", "high", "false", "no"));
            trainingData.AddRow(ListOf("sunny", "cool", "normal", "false", "yes"));
            trainingData.AddRow(ListOf("rainy", "mild", "normal", "false", "yes"));

            var validationData = new DataTable(ListOf("Outlook", "Temprature", "Humidity", "Windy", "Play"));
            validationData.AddRow(ListOf("sunny", "mild", "normal", "true", "yes"));
            validationData.AddRow(ListOf("overcast", "mild", "high", "true", "yes"));
            validationData.AddRow(ListOf("overcast", "hot", "normal", "false", "yes"));
            validationData.AddRow(ListOf("rainy", "mild", "high", "true", "no"));


            var treeBuilder = new C45TreeBuilder();
            var tree = treeBuilder.BuildTree(trainingData, classifier: "Play");

            int totalPredictions = 0;
            int correctPredictions = 0;

            foreach (var row in validationData.Rows())
            {
                string @class = row["Play"];
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
