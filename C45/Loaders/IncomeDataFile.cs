using C45.Loaders.DataFunctions;
using System;
using System.Collections.Generic;

namespace C45.Loaders
{
    public class IncomeDataFile : IDataFile
    {
        private const string MissingValue = "?";
        private const string DateFilePath = "Resources/income_evaluation.csv";
        private readonly IDataFile _dataFile;

        public IncomeDataFile()
        {
            _dataFile = new ReadDataFile(DateFilePath, "income")
                .NormalizeAttributes()
                .NormalizeRecords()
                .FilterRecords(IsNotMissingValue)
                .ShuffleRecords()
                .LimitRecords(5000)
                .RemoveColumns("fnlwgt", "relationship", "education-num", "capital-gain", "capital-loss", "hours-per-week")
                .GroupColumn("age", ToAgeGroups)
                .GroupColumn("native-country", ToRegions);

            _dataFile.Dump(DateFilePath + $"_{DateTime.Now.Ticks}.txt");
        }


        public IEnumerable<string> Attributes => _dataFile.Attributes;

        public IEnumerable<IList<string>> Records => _dataFile.Records;

        public string ClassificationAttribute => _dataFile.ClassificationAttribute;

        private static bool IsNotMissingValue(IList<string> record)
        {
            return record.Contains(MissingValue);
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

        private static string ToRegions(string countryName)
        {
            // Age bands taken from https://www.ons.gov.uk/aboutus/transparencyandgovernance/freedomofinformationfoi/ukaverageincomebyagebands

            var dictionary = new Dictionary<string, string>
            {
                ["china"] = "asia",
                ["india"] = "asia",
                ["iran"] = "asia",
                ["japan"] = "asia",
                ["laos"] = "asia",
                ["philippines"] = "asia",
                ["taiwan"] = "asia",
                ["thailand"] = "asia",
                ["vietnam"] = "asia",
                ["cuba"] = "central america",
                ["dominican-republic"] = "central america",
                ["el-salvador"] = "central america",
                ["guatemala"] = "central america",
                ["haiti"] = "central america",
                ["honduras"] = "central america",
                ["jamaica"] = "central america",
                ["mexico"] = "central america",
                ["nicaragua"] = "central america",
                ["puerto-rico"] = "central america",
                ["england"] = "europe",
                ["france"] = "europe",
                ["germany"] = "europe",
                ["greece"] = "europe",
                ["holand-netherlands"] = "europe",
                ["hungary"] = "europe",
                ["ireland"] = "europe",
                ["italy"] = "europe",
                ["poland"] = "europe",
                ["portugal"] = "europe",
                ["scotland"] = "europe",
                ["yugoslavia"] = "europe",
                ["hong"] = "hong",
                ["canada"] = "north america",
                ["united-states"] = "north america",
                ["cambodia"] = "south america",
                ["columbia"] = "south america",
                ["ecuador"] = "south america",
                ["peru"] = "south america",
                ["trinadad & tobago"] = "south america"
            };

            return dictionary.ContainsKey(countryName) ? dictionary[countryName] : countryName;
        }
    }
}
