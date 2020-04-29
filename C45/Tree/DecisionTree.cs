using C45.Data;

namespace C45.Tree
{
    public interface IDecisionTree
    {
        public string Classify(DataTable.Row row);
    }
}
