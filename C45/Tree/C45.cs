using C45.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Tree
{
    public class C45
    {
        private readonly IGainFunc _gain;

        public C45(IGainFunc gain)
        {
            _gain = gain;
        }

        public IDecisionTree BuildTree(IDataTable data, string classAttribute)
        {
            var classCounts = new ClassCounts(data, classAttribute);
            var valueCounts = data.Attributes
                .Where(attribute => attribute != classAttribute)
                .ToDictionary(attribute => attribute,
                              attribute => new ValueClassCounts(data, attribute, classAttribute));

            var attribute = _gain.GetAttributeWithHighestGain(data.RowCount, valueCounts, classCounts);
                        
            DecisionTreeNode node = new DecisionTreeNode(attribute);
            //foreach (var value in attribute)
            //{
            //    // Base case where value for attribute only has a single classification.
            //    // In this case we always return the classification.
            //    if (value.Classifications.Count == 1)
            //    {
            //        node[value] = new DecisionTreeLeafNode(value.Classifications.Single());
            //    }
            //    // Default case where we drill down and build a new node.
            //    else
            //    {
            //        IDataTable newData = data.DrillDown(attribute, value);
            //        node[value] = BuildTree(newData, classAttribute);
            //    }
            //}

            return node;
        }

      

        private class DecisionTreeNode : IDecisionTree
        {
            private readonly string _attribute;

            public DecisionTreeNode(string attribute)
            {
                _attribute = attribute;
            }

            public IDictionary<string, IDecisionTree> Children { get; } = new Dictionary<string, IDecisionTree>();

            public string Classify(IDataTableRow row)
            {
                throw new NotImplementedException();
            }
        }

        private class DecisionTreeLeafNode : IDecisionTree
        {
            private readonly string _classification;

            public DecisionTreeLeafNode(string classification)
            {
                _classification = classification;
            }

            public string Classify(IDataTableRow row)
            {
                return _classification;
            }
        }
    }
}
