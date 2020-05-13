using C45.Data;
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

                // handle unseen value (Base case 3?)
                if (!Children.ContainsKey(value))
                {
                    // store most common value for use later
                    if (!Children.ContainsKey("__default"))
                    {
                        Children["__default"] = new DecisionTreeLeafNode(this.MostCommonOutcome());
                    }

                    return Children["__default"].Classify(row);
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

        public static string MostCommonOutcome(this IDecisionTree decisionTree)
        {
            var counts = new Dictionary<string, int>();
            decisionTree.CountOutcomes(counts);

            return counts.Select(x => (x.Key, x.Value))
                .ToList()
                .OrderByDescending(x => x.Value)
                .First()
                .Key;
        }

        public static void CountOutcomes(this IDecisionTree decisionTree, IDictionary<string, int> counts)
        {
            if (decisionTree is IDecisionNode decisionNode)
            {
                foreach (var child in decisionNode.Children)
                {
                    child.Value.CountOutcomes(counts);
                }
            }
            else
            {
                if (!counts.ContainsKey(decisionTree.Label))
                {
                    counts[decisionTree.Label] = 1;
                }
                else
                {
                    counts[decisionTree.Label]++;
                }
            }
        }
    }
}
