using C45.Data;
using System.Collections.Generic;
using System.Linq;

namespace C45.Tree
{
    public class C45TreeBuilder
    {
        public IDecisionTree BuildTree(DataTable data, string targetAttribute)
        {
            var classes = data.GetDistinctValuesForAttribute(targetAttribute);
            if (classes.Count == 1)
            {
                return new DecisionTreeLeafNode(classes.Single());
            }
            else if (data.Attributes.Count() == 1)
            {
                return new DecisionTreeLeafNode(GetMostCommonOutcome(data, targetAttribute));
            }
            else
            {
                var attributeGains = data.GetGainForEachAttribute(targetAttribute);
                // do something if all gains are 0

                return HandleDefaultCase(data, attributeGains, targetAttribute);
            }
        }

        private static string GetMostCommonOutcome(DataTable data, string targetAttribute)
        {
            return data.Rows()
                .GroupBy(x => x[targetAttribute])
                .OrderByDescending(x => x.Key)
                .Select(x => x.First()[targetAttribute])
                .First();
        }

        private IDecisionTree HandleDefaultCase(DataTable data, IList<(string Attribute, double Gain)> attributeGains, string targetAttribute)
        {
            var attributeWithHighestGain = attributeGains.GetAttributeWithHighestGain();
            var valuesForAttribute = data.GetDistinctValuesForAttribute(attributeWithHighestGain);

            var attributeNode = new DecisionTreeNode(attributeWithHighestGain);

            foreach (var value in valuesForAttribute)
            {
                var newData = data.DrillDown(attributeWithHighestGain, value);
                attributeNode.Children[value] = BuildTree(newData, targetAttribute);
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
            public DecisionTreeNode(string attribute)
            {
                Label = attribute;
            }

            public string Label { get; }

            public IDictionary<string, IDecisionTree> Children { get; } = new Dictionary<string, IDecisionTree>();

            public string Classify(DataTable.Row row)
            {
                var value = row[Label];

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
            public DecisionTreeLeafNode(string classification)
            {
                Label = classification;
            }

            public string Label { get; }

            public string Classify(DataTable.Row row)
            {
                return Label;
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
