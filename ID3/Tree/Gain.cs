using C45.Data;
using ID3.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Tree
{
    public static class Gain
    {
        public static double CalculateGainForAttribute(DataTable data, string attribute, string targetAttribute)
        {
            return CalculateEntropyForDataset(data, targetAttribute) - CalculateTotalEntropyForAttribute(data, attribute, targetAttribute);
        }

        public static double CalculateTotalEntropyForAttribute(DataTable data, string attribute, string targetAttribute)
        {
            double totalEntropy = 0.0;

            var values = data.GetDistinctValuesForAttribute(attribute);
            foreach (var value in values)
            {
                int sizeOfSubset = data.CountValueOccurrences(attribute, value);
                double valueEntropy = CalculateEntropyForSubset(data, attribute, value, sizeOfSubset, targetAttribute);
                totalEntropy += valueEntropy * ((double)sizeOfSubset / data.RowCount);
            }

            return totalEntropy;
        }

        public static double CalculateEntropyForSubset(DataTable data, string attribute, string value, int sizeOfSubset, string targetAttribute)
        {
            // Calculates entropy for a subset of the data, containing records that have the given `value` for the specified `attribute`

            double entropy = 0.0;

            var outcomes = data.GetDistinctOutcomes(attribute, value, targetAttribute);
            foreach (var outcome in outcomes)
            {
                int outcomeCount = data.CountOutcomeOccurrences(attribute, value, targetAttribute, outcome);
                entropy += PartialEntropyValue(outcomeCount, sizeOfSubset);
            }

            return entropy;
        }

        public static double CalculateEntropyForDataset(DataTable data, string targetAttribute)
        {
            double entropy = 0.0;

            var classes = data.GetDistinctValuesForAttribute(targetAttribute);
            foreach (var @class in classes)
            {
                int classOccurrances = data.CountValueOccurrences(targetAttribute, @class);
                entropy += PartialEntropyValue(classOccurrances, data.RowCount);
            }

            return entropy;
        }

        private static double PartialEntropyValue(int valueOccuranceCount, int setSize)
        {
            var a = (double)valueOccuranceCount / setSize;
            return -a * Math.Log2(a);
        }
    }

    public static class GainHelpers
    {
        public static IList<(string Attribute, double Gain)> GetGainForEachAttribute(this DataTable data, string targetAttribute)
        {
            var attributeGains = new List<(string Attribute, double Gain)>();
            foreach (var attribute in data.Attributes.Where(x => x != targetAttribute))
            {
                attributeGains.Add((Attribute: attribute, Gain: Gain.CalculateGainForAttribute(data, attribute, targetAttribute)));
            }
            return attributeGains;
        }

        public static string GetAttributeWithHighestGain(this IList<(string Attribute, double Gain)> attributeGains)
        {
            return attributeGains.OrderByDescending(x => x.Gain).First().Attribute;
        }
    }
}
