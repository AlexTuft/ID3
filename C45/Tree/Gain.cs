using System;
using System.Collections.Generic;

namespace C45.Tree
{
    public interface IGainFunc
    {
        string GetAttributeWithHighestGain(int recordCount, IDictionary<string, ValueClassCounts> attributesValueCounts, ClassCounts classCounts);
    }

    partial class Gain : IGainFunc
    {
        public string GetAttributeWithHighestGain(int recordCount, IDictionary<string, ValueClassCounts> attributesValueCounts, ClassCounts classCounts)
        {
            string highestGainAttribute = "";
            double hightestGainValue = 0.0;

            double classAttributeEntropy = CalculateEntopyForClass(recordCount, classCounts);

            foreach (var attributeAndCounts in attributesValueCounts)
            {
                double gain = CalculateGainForAttribute(recordCount, attributeAndCounts.Value, classAttributeEntropy);
                if (gain > hightestGainValue)
                {
                    highestGainAttribute = attributeAndCounts.Key;
                    hightestGainValue = gain;
                }
            }

            return highestGainAttribute;
        }

        private static double CalculateEntopyForClass(int recordCount, ClassCounts classCounts)
        {
            double classAttributeEntropy = 0.0;
            foreach (var @class in classCounts.Classes)
            {
                var a = (double)classCounts.CountOf(@class) / recordCount;
                classAttributeEntropy += -a * Math.Log2(a);
            }

            return classAttributeEntropy;
        }

        private double CalculateGainForAttribute(int recordCount, ValueClassCounts valueCounts, double classAttributeEntropy)
        {
            Dictionary<string, double> valueEntropies = CalculateEntropiesForValuesInAttribute(valueCounts);
            double attributeEntropy = CalculateEntropyForAttribute(recordCount, valueCounts, valueEntropies);
            return classAttributeEntropy - attributeEntropy;
        }

        private static Dictionary<string, double> CalculateEntropiesForValuesInAttribute(ValueClassCounts valueCounts)
        {
            var valueEntropies = new Dictionary<string, double>();
            foreach (var value in valueCounts.UniqueValues)
            {
                double entropy = 0.0;
                foreach (var @class in valueCounts.ClassesFor(value))
                {
                    var a = (double)valueCounts.CountOfClass(value, @class) / valueCounts.CountOfValue(value);
                    entropy += -a * Math.Log2(a);
                }
                valueEntropies[value] = entropy;
            }

            return valueEntropies;
        }

        private static double CalculateEntropyForAttribute(int recordCount, ValueClassCounts valueCounts, Dictionary<string, double> valueEntropies)
        {
            double attributeEntropy = 0.0;
            foreach (var valueEntropy in valueEntropies)
            {
                attributeEntropy += ((double)valueCounts.CountOfValue(valueEntropy.Key) / recordCount) * valueEntropy.Value;
            }

            return attributeEntropy;
        }
    }
}
