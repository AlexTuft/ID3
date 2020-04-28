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
            var classSummary = new ClassSummary(data, classAttribute);

            if (classSummary.AllClassesAreSame())
            {
                return CreateLeafNode(classSummary);
            }
            else
            {
                return HandleDefaultCase(data, classSummary, classAttribute);
            }
        }

        private DecisionTreeNode HandleDefaultCase(IDataTable data, ClassSummary classSummary, string classAttribute)
        {
            var bestAttribute = GetAttributeWithHighestGain(data, classSummary, classAttribute);
            var attributeNode = new DecisionTreeNode(bestAttribute.Name);
            
            foreach (var value in bestAttribute.UniqueValues)
            {
                IDataTable newData = data.DrillDown(bestAttribute.Name, value);
                attributeNode.Children[value] = BuildTree(newData, classAttribute);
            }

            return attributeNode;
        }

        private AttributeData GetAttributeWithHighestGain(IDataTable data, ClassSummary classSummary, string classAttribute)
        {
            var attributeDatas = GetAttributeData(data, classAttribute);
            var attributeWithHighestGain = _gain.GetAttributeWithHighestGain(data.RowCount, attributeDatas, classSummary);
            return attributeDatas[attributeWithHighestGain];
        }

        private static Dictionary<string, AttributeData> GetAttributeData(IDataTable data, string classAttribute)
        {
            return data.Attributes
                .Where(attribute => attribute != classAttribute)
                .ToDictionary(attribute => attribute,
                                attribute => new AttributeData(data, attribute, classAttribute));
        }

        private static DecisionTreeLeafNode CreateLeafNode(ClassSummary classSummary)
        {
            return new DecisionTreeLeafNode(classSummary.Classes.Single());
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
