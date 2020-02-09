using C45.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Tree
{
    public class C45TreeBuilder
    {
        private readonly IGainFunc _gain;

        public C45TreeBuilder(IGainFunc gain)
        {
            _gain = gain;
        }

        public IDecisionTree BuildTree(IDataTable data, string classAttribute)
        {
            var classData = new ClassSummary(data, classAttribute);

            // Handle base case where all classes are the same
            if (classData.Classes.Count() == 1)
            {
                return new DecisionTreeLeafNode(classData.Classes.Single());
            }

            var attributeDatas = data.Attributes
                .Where(attribute => attribute != classAttribute)
                .ToDictionary(attribute => attribute,
                              attribute => new AttributeSummary(data, attribute, classAttribute));

            var attribute = _gain.GetAttributeWithHighestGain(data.RowCount, attributeDatas, classData);
            var attributeData = attributeDatas[attribute];
            var attributeNode = new DecisionTreeNode(attribute);

            foreach (var value in attributeData.UniqueValues)
            {
                // Handle base case where all classes are the same
                if (attributeData.ClassesFor(value).Count() == 1)
                {
                   attributeNode.Children[value] = new DecisionTreeLeafNode(attributeData.ClassesFor(value).Single());
                }
                // Default case where we drill down and build a new node.
                else
                {
                    IDataTable newData = data.DrillDown(attribute, value);
                    attributeNode.Children[value] = BuildTree(newData, classAttribute);
                }
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

            public string Classify(IDataTableRow row)
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

            public string Classify(IDataTableRow row)
            {
                return _classification;
            }
        }
    }
}
