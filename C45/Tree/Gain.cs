using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C45.Tree
{
    public interface IGainFunc
    {
        string CalculateHighestGain(IDataTable data, string classAttribute);
    }

    class Gain : IGainFunc
    {
        public string CalculateHighestGain(IDataTable data, string classAttribute)
        {
            string highestGainAttribute = "";
            double hightestGainValue = 0.0;

            var dataSummary = new DataSummary(data, classAttribute);

            foreach (var attribute in dataSummary.AttributeSummaries)
            {
                double gain = CalculateGainForAttribute(data, attribute, classAttribute);
                if (gain > hightestGainValue)
                {
                    highestGainAttribute = attribute;
                    hightestGainValue = gain;
                }
            }

            return highestGainAttribute;
        }

        private double CalculateGainForAttribute()
        {
            var valueEntropies = new Dictionary<string, double>();

            foreach (var valueCount in valueCounts)
            {
                double entropy = 0.0;
                foreach (var classCount in valueClassCounts[valueCount.Key])
                {
                    var a = (double)classCount.Value / valueCount.Value;
                    entropy += -a * Math.Log2(a);
                }
                valueEntropies[valueCount.Key] = entropy;
            }

            double attributeEntropy = 0.0;
            foreach (var valueEntropy in valueEntropies)
            {
                attributeEntropy += ((double)valueCounts[valueEntropy.Key] / totalCount) * valueEntropy.Value;
            }

            double classifierAttributeEntropy = 0.0;
            foreach (var classCount in classifierCounts)
            {
                var a = (double)classCount.Value / totalCount;
                classifierAttributeEntropy += -a * Math.Log2(a);
            }

            var gain = classifierAttributeEntropy - attributeEntropy;
            return gain;
        }
    }
}
