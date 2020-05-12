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

        private IDecisionTree HandleDefaultCase(DataTable data, IList<(string Attribute, double Gain)> attributeGains, string classifier)
        {
            var attributeWithHighestGain = attributeGains.GetAttributeWithHighestGain();
            var valuesForAttribute = data.GetUniqueValueForAttribute(attributeWithHighestGain);

            if (valuesForAttribute.Count == 1)
            {
                var newData = data.DrillDown(attributeWithHighestGain, valuesForAttribute.Single());
                return BuildTree(newData, classifier);
            }

            var attributeNode = new DecisionTreeNode(attributeWithHighestGain);
            
            foreach (var value in valuesForAttribute)
            {
                var newData = data.DrillDown(attributeWithHighestGain, value);
                attributeNode.Children[value] = BuildTree(newData, classifier);
            }

            var single = "";
            if (attributeNode.AreAllOutcomesIdentical(ref single))
            {
                return new DecisionTreeLeafNode(single);
            }

            return attributeNode;
        }

        internal class DecisionTreeNode : IDecisionNode
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

        internal class DecisionTreeLeafNode : ILeafNode
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

    public static class IDecisionTreeHelpers
    {
        public static bool AreAllOutcomesIdentical(this IDecisionTree decisionTree, ref string single)
        {
            var outcomes = decisionTree.GetAllOutcomes();
            if (outcomes.Count() == 1)
            {
                single = outcomes.Single();
                return true;
            }
            return false;
        }

        public static ISet<string> GetAllOutcomes(this IDecisionTree decisionTree)
        {
            var outcomes = new HashSet<string>();

            if (decisionTree is IDecisionNode decisionNode)
            {
                foreach (var child in decisionNode.Children)
                {
                    outcomes.UnionWith(child.Value.GetAllOutcomes());
                }
            }
            else
            {
                outcomes.Add(decisionTree.Label);
            }

            return outcomes;
        }
    }
}
