using C45.Data;
using System.Collections.Generic;

namespace C45.Tree
{
    public interface IDecisionTree
    {
        public string Label { get; }

        public string Classify(DataTable.Row row);
    }

    public interface IDecisionNode : IDecisionTree
    {
        IDictionary<string, IDecisionTree> Children { get; }
    }

    public interface ILeafNode : IDecisionTree
    {

    }
}
