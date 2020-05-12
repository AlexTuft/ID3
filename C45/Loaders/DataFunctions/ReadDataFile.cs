﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace C45.Loaders.DataFunctions
{
    public class ReadDataFile : IDataFile
    {
        public ReadDataFile(string path)
        {
            var attributesAndRecords = GetAttributesAndRecords(path);
            Attributes = attributesAndRecords.Attributes;
            Records = attributesAndRecords.Records;
        }

        public IList<string> Attributes { get; }

        public IEnumerable<IList<string>> Records { get; }

        public string ClassificationAttribute => "income";

        private static (IList<string> Attributes, IEnumerable<IList<string>> Records) GetAttributesAndRecords(string path)
        {
            using var file = new StreamReader(new FileStream(path, FileMode.Open));
            return (ReadAttributes(file), ReadRecords(file));
        }

        private static IList<string> ReadAttributes(StreamReader file) =>
            file.CSVReadLine()
                .ToList();

        private static IList<IList<string>> ReadRecords(StreamReader file)
        {
            return file.CSVLines()
                .ToList();
        }
    }
}
