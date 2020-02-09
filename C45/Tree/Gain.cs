using C45.Data;
using System;
using System.Collections.Generic;

namespace C45.Tree
{
    public interface IGainFunc
    {
        string GetAttributeWithHighestGain(int recordCount, IDictionary<string, AttributeSummary> attributeDatas, ClassSummary classData);
    }

    partial class Gain : IGainFunc
    {
        public string GetAttributeWithHighestGain(int recordCount, IDictionary<string, AttributeSummary> attributeDatas, ClassSummary classData)
        {
            string highestGainAttribute = "";
            double hightestGainValue = 0.0;

            double classAttributeEntropy = CalculateEntopyForClass(recordCount, classData);

            foreach (var attributeAndData in attributeDatas)
            {
                double gain = CalculateGainForAttribute(recordCount, attributeAndData.Value, classAttributeEntropy);
                if (gain > hightestGainValue)
                {
                    highestGainAttribute = attributeAndData.Key;
                    hightestGainValue = gain;
                }
            }

            return highestGainAttribute;
        }

        private static double CalculateEntopyForClass(int recordCount, ClassSummary classData)
        {
            double classAttributeEntropy = 0.0;
            foreach (var @class in classData.Classes)
            {
                var a = (double)classData.CountOf(@class) / recordCount;
                classAttributeEntropy += -a * Math.Log2(a);
            }

            return classAttributeEntropy;
        }

        private double CalculateGainForAttribute(int recordCount, AttributeSummary attributeData, double classAttributeEntropy)
        {
            Dictionary<string, double> valueEntropies = CalculateEntropiesForValuesInAttribute(attributeData);
            double attributeEntropy = CalculateEntropyForAttribute(recordCount, attributeData, valueEntropies);
            return classAttributeEntropy - attributeEntropy;
        }

        private static Dictionary<string, double> CalculateEntropiesForValuesInAttribute(AttributeSummary attributeData)
        {
            var valueEntropies = new Dictionary<string, double>();
            foreach (var value in attributeData.UniqueValues)
            {
                double entropy = 0.0;
                foreach (var @class in attributeData.ClassesFor(value))
                {
                    var a = (double)attributeData.CountOfClass(value, @class) / attributeData.CountOfValue(value);
                    entropy += -a * Math.Log2(a);
                }
                valueEntropies[value] = entropy;
            }

            return valueEntropies;
        }

        private static double CalculateEntropyForAttribute(int recordCount, AttributeSummary attributeData, Dictionary<string, double> valueEntropies)
        {
            double attributeEntropy = 0.0;
            foreach (var valueEntropy in valueEntropies)
            {
                attributeEntropy += ((double)attributeData.CountOfValue(valueEntropy.Key) / recordCount) * valueEntropy.Value;
            }

            return attributeEntropy;
        }
    }
}
