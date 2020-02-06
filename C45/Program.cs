using System;
using System.Data;

namespace C45
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable trainingData = new DataTable(ListOf("Outlook", "Temprature", "Humidity", "Windy", "Play"));
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

            DataTable validationData = new DataTable(ListOf("Outlook", "Temprature", "Humidity", "Windy", "Play"));
            trainingData.AddRow(ListOf("sunny", "mild", "normal", "true", "yes"));
            trainingData.AddRow(ListOf("overcast", "mild", "high", "true", "yes"));
            trainingData.AddRow(ListOf("overcast", "hot", "normal", "false", "yes"));
            trainingData.AddRow(ListOf("rainy", "mild", "high", "true", "no"));

            var gain = new Gain();
            var outlookGain = gain.CalculateHighestGain(trainingData, "Play");

            DecisionTreeBuilder treeBuilder = new DecisionTreeBuilder(new Entropy());

            DecisionTree tree = treeBuilder.BuildTree(trainingData, classificationAttribute: "Play");

            int totalPredictions = 0;
            int correctPredictions = 0;

            foreach (IDataTableRow row in validationData)
            {
                string classification = row.Get("Play");
                string predictedClassification = tree.Classify(row);

                if (classification == predictedClassification)
                {
                    correctPredictions++;
                }
                totalPredictions++;
            }

            Console.WriteLine($"Total predictions: {totalPredictions}, correct: {correctPredictions} ({(int)(correctPredictions / totalPredictions * 100)}%)");
        }

        private static SerializationInfo ListOf(string v1, string v2, string v3, string v4, string v5)
        {
            throw new NotImplementedException();
        }
    }
}
