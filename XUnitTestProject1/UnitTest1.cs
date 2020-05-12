using C45.Data;
using Xunit;

using static C45.Common.ListHelpers;

namespace XUnitTestProject1
{

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            DataTable data = new DataTable(ListOf("1", "2"));
            data.AddRow(ListOf("a", "X"));
            data.AddRow(ListOf("a", "X"));
            data.AddRow(ListOf("a", "X"));
            data.AddRow(ListOf("a", "Y"));
            data.AddRow(ListOf("b", "X"));
            data.AddRow(ListOf("b", "X"));
            data.AddRow(ListOf("b", "X"));
            data.AddRow(ListOf("b", "Y"));
        }
    }
}
