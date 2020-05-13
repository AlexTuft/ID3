using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace C45.Data
{
    public class DataTable
    {
        private readonly IList<string> _attributes;
        private readonly List<IList<string>> _data;

        public DataTable(IList<string> attributes)
        {
            _attributes = attributes;
            _data = new List<IList<string>>();
        }

        public int RowCount => _data.Count;

        public IEnumerable<string> Attributes => _attributes;

        public void AddRow(IList<string> row)
        {
            if (row.Count != _attributes.Count)
            {
                throw new ArgumentException($"Number of items must be {_attributes.Count} but was {row.Count}.", nameof(row));
            }
            _data.Add(row);
        }

        public void AddRows(IEnumerable<IList<string>> rows)
        {
            foreach (var row in rows)
            {
                AddRow(row);
            }
        }

        private void AddRow(DataTable.Row row)
        {
            var values = new List<string>();
            foreach (var attribute in _attributes)
            {
                values.Add(row[attribute]);
            }
            AddRow(values);
        }

        public IEnumerable<Row> Rows()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                yield return new Row(this, i);
            }
        }

        public DataTable DrillDown(string attribute, string value)
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

        public class Row : IEnumerable<string>
        {
            private readonly DataTable _table;
            private readonly int _rowIndex;

            public Row(DataTable table, int rowIndex)
            {
                _table = table;
                _rowIndex = rowIndex;
            }

            public int Columns => _table.Attributes.Count();

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

            public IEnumerator<string> GetEnumerator()
            {
                foreach (var attribute in _table.Attributes)
                {
                    yield return this[attribute];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
