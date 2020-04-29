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

        private class DecisionTreeNode : IDecisionTree
        {
            private readonly string _attribute;

            public DecisionTreeNode(string attribute)
            {
                _attribute = attribute;
            }

            public IDictionary<string, IDecisionTree> Children { get; } = new Dictionary<string, IDecisionTree>();

            public string Classify(DataTable.Row row)
            {
                var value = row[_attribute];
                return Children[value].Classify(row);
            }
        }

        private class DecisionTreeLeafNode : IDecisionTree
        {
            private readonly string _classification;

            public DecisionTreeLeafNode(string classification)
            {
                _classification = classification;
            }

            public string Classify(DataTable.Row row)
            {
                return _classification;
            }
        }
    }
}
