using C45.Data;
using System;

namespace C45
{
    public static class Gain
    {
        public static double CalculateGainForAttribute(DataTable data, string attribute, string classifier)
        {
            return CalculateTotalEntropy(data, classifier) - CalculateEntropyForAttribute(data, attribute, classifier);
        }

        public static double CalculateEntropyForAttribute(DataTable data, string attribute, string classifier)
        {
            double entropy = 0.0;

            var values = data.GetUniqueValueForAttribute(attribute);
            foreach (var value in values)
            {
                int valueOccurrances = data.CountValueOccurrences(attribute, value);
                double valueEntropy = CalculateEntropyForValueOfAttribute(data, attribute, value, valueOccurrances, classifier);
                entropy += valueEntropy * ((double)valueOccurrances / data.RowCount);
            }

            return entropy;
        }

        public static double CalculateEntropyForValueOfAttribute(DataTable data, string attribute, string value, int valueOccurrances, string classifier)
        {
            double entropy = 0.0;

            var classes = data.GetClassesForValue(attribute, value, classifier);
            foreach (var @class in classes)
            {
                int classOccurrances = data.CountClassOccurrences(attribute, value, classifier, @class);
                entropy += PartialEntropyValue((double)classOccurrances / valueOccurrances);
            }

            return entropy;
        }

        public static double CalculateTotalEntropy(DataTable data, string classifier)
        {
            double entropy = 0.0;

            var classes = data.GetUniqueValueForAttribute(classifier);
            foreach (var @class in classes)
            {
                int classOccurrances = data.CountValueOccurrences(classifier, @class);
                entropy += PartialEntropyValue((double)classOccurrances / data.RowCount);
            }

            return entropy;
        }

        private static double PartialEntropyValue(double a)
        {
            return -a * Math.Log2(a);
        }
    }
}
