using System;
using System.Collections.Generic;
using System.Linq;

namespace C45.Data
{
    public interface IDataTable
    {
        IEnumerable<string> Attributes { get; }

        IDataTable DrillDown(string attribute, string value);
        IEnumerable<IDataTableRow> Rows();
    }

    public interface IDataTableRow
    {
        string this[string attribute] {get; }
    }
    
    public class DataTable : IDataTable
    {
        private readonly IList<string> _attributes;
        private readonly List<IList<string>> _data;

        public DataTable(IList<string> attributes)
        {
            _attributes = attributes;
            _data = new List<IList<string>>();
        }

        public IEnumerable<string> Attributes => _attributes;

        public void AddRow(IList<string> row)
        {
            if (row.Count != _attributes.Count) {
                throw new ArgumentException($"Number of items must be {_attributes.Count} but was {row.Count}.", nameof(row));
            }
            _data.Add(row);
        }

        private void AddRow(IDataTableRow row)
        {
            var values = new List<string>();
            foreach (var attribute in _attributes)
            {
                values.Add(row[attribute]);
            }
            AddRow(values);
        }

        public IEnumerable<IDataTableRow> Rows()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                yield return new Row(this, i);
            }
        }

        public IDataTable DrillDown(string attribute, string value)
        {
            DataTable newTable = new DataTable(_attributes.Where(x => x != attribute).ToList());

            foreach (var row in Rows())
            {
                if (row[attribute] == value)
                {
                    newTable.AddRow(row);
                }
            }

            return newTable;
        }

        private class Row : IDataTableRow
        {
            private readonly DataTable _table;
            private readonly int _rowIndex;

            public Row(DataTable table, int rowIndex)
            {
                _table = table;
                _rowIndex = rowIndex;
            }

            public string this[string attribute]
            {
                get
                {
                    int attributeIndex = _table._attributes.IndexOf(attribute);
                    if (attributeIndex == -1)
                    {
                        throw new ArgumentException($"DataTable does not have attribute '{attribute}'.", nameof(attribute));
                    }
                    return _table._data[_rowIndex][attributeIndex];
                }
            }
        }
    }
}
