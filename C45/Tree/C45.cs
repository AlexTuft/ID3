using C45.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Tree
{
    public class C45TreeBuilder
    {
        public IDecisionTree BuildTree(DataTable data, string classifier)
        {
            var classes = data.GetUniqueValueForAttribute(classifier);
            if (classes.Count == 1)
            {
                return new DecisionTreeLeafNode(classes.Single());
            }
            else if (data.Attributes.Count() == 1)
            {
                return new DecisionTreeLeafNode(data.Rows()
                    .GroupBy(x => x[classifier])
                    .OrderByDescending(x => x.Key)
                    .Select(x => x.First()[classifier])
                    .First());
            }
            else
            {
                var attributeGains = data.GetGainForEachAttribute(classifier);
                // do something if all gains are 0

                return HandleDefaultCase(data, attributeGains, classifier);
            }
        }

        private DecisionTreeNode HandleDefaultCase(DataTable data, IList<(string Attribute, double Gain)> attributeGains, string classifier)
        {
            var attributeWithHighestGain = attributeGains.GetAttributeWithHighestGain();
            var valuesForAttribute = data.GetUniqueValueForAttribute(attributeWithHighestGain);

            var attributeNode = new DecisionTreeNode(attributeWithHighestGain);
            
            foreach (var value in valuesForAttribute)
            {
                var newData = data.DrillDown(attributeWithHighestGain, value);
                attributeNode.Children[value] = BuildTree(newData, classifier);
            }

            return attributeNode;
        }

        internal class DecisionTreeNode : IDecisionTree
        {
            private readonly string _attribute;

            public DecisionTreeNode(string attribute)
            {
                _attribute = attribute;
            }

            public string Label => _attribute;

            public IDictionary<string, IDecisionTree> Children { get; } = new Dictionary<string, IDecisionTree>();

            public string Classify(DataTable.Row row)
            {
                var value = row[_attribute];
                
                // handle unseen value
                if (!Children.ContainsKey(value))
                {
                    throw new ClassificationException($"Unseen value ({value}) for attribute {_attribute}.");
                }

                return Children[value].Classify(row);
            }
        }

        internal class DecisionTreeLeafNode : IDecisionTree
        {
            private readonly string _classification;

            public DecisionTreeLeafNode(string classification)
            {
                _classification = classification;
            }

            public string Label => _classification;

            public string Classify(DataTable.Row row)
            {
                return _classification;
            }
        }
    }
}
