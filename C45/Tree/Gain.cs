using C45.Data;
using System;
using System.Collections.Generic;

namespace C45.Tree
{
    public interface IGainFunc
    {
        string GetAttributeWithHighestGain(int recordCount, IDictionary<string, AttributeSummary> attributeDatas, ClassSummary classData);
    }

    public class Gain : IGainFunc
    {
        public string GetAttributeWithHighestGain(int recordCount, IDictionary<string, AttributeSummary> attributeDatas, ClassSummary classData)
        {
            string bestAttribute = "";
            double highestGain = 0.0;

            double classAttributeEntropy = GetEntopyForClass(recordCount, classData);

            foreach (var attributeAndData in attributeDatas)
            {
                double gain = GetGainForAttribute(recordCount, attributeAndData.Value, classAttributeEntropy);
                if (gain > highestGain)
                {
                    bestAttribute = attributeAndData.Key;
                    highestGain = gain;
                }
            }

            return bestAttribute;
        }

        private static double GetEntopyForClass(int recordCount, ClassSummary classData)
        {
            double classAttributeEntropy = 0.0;
            foreach (var @class in classData.Classes)
            {
                var a = (double)classData.CountOf(@class) / recordCount;
                classAttributeEntropy += -a * Math.Log2(a);
            }

            return classAttributeEntropy;
        }

        private static double GetGainForAttribute(int recordCount, AttributeSummary attributeData, double classAttributeEntropy)
        {
            var valueEntropies = GetEntropiesForValuesOfAttribute(attributeData);
            return classAttributeEntropy - GetEntropyForAttribute(recordCount, attributeData, valueEntropies);
        }

        private static Dictionary<string, double> GetEntropiesForValuesOfAttribute(AttributeSummary attributeData)
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

        private static double GetEntropyForAttribute(int recordCount, AttributeSummary attributeData, Dictionary<string, double> valueEntropies)
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
