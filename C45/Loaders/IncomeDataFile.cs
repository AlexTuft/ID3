using C45.Loaders.DataFunctions;
using System;
using System.Collections.Generic;

namespace C45.Loaders
{
    public class IncomeDataFile : IDataFile
    {
        private const string MissingValue = "?";

        private readonly IDataFile _dataFile;

        public IncomeDataFile()
        {
            _dataFile = new ReadDataFile("Resources/income_evaluation.csv")
                .ShuffleRecords()
                .NormalizeAttributes()
                .FilterRecords("workclass", IsNotMissingValue)
                .LimitRecords(5000)
                .RemoveColumn("fnlwgt")
                .RemoveColumn("relationship")
                .RemoveColumn("education-num")
                .RemoveColumn("capital-gain")
                .RemoveColumn("capital-loss")
                .RemoveColumn("hours-per-week")
                .NormalizeRecords()
                .GroupColumn("age", ToAgeGroups);
        }

        private static bool IsNotMissingValue(string x)
        {
            return !string.Equals(x.Trim(), MissingValue, StringComparison.Ordinal);
        }

        private static string ToAgeGroups(string ageText)
        {
            // Age bands taken from https://www.ons.gov.uk/aboutus/transparencyandgovernance/freedomofinformationfoi/ukaverageincomebyagebands
            string ageGroup = "";

            int age = int.Parse(ageText);
            if (age < 18)
            {
                ageGroup = "<18";
            }
            else if (age < 26)
            {
                ageGroup = "18-25";
            }
            else if (age < 31)
            {
                ageGroup = "26-30";
            }
            else if (age < 36)
            {
                ageGroup = "31-35";
            }
            else
            {
                ageGroup = "35+";
            }

            return ageGroup;
        }

        public IList<string> Attributes => _dataFile.Attributes;

        public IEnumerable<IList<string>> Records => _dataFile.Records;

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;
    }
}
