using System.Collections.Generic;

namespace C45.Loaders
{
    public class WeatherData : IDataFile
    {
        public WeatherData()
        {
            Attributes = new List<string> { "Outlook", "Temprature", "Humidity", "Windy", "Play" };
            Records = new List<IList<string>>
            {
                new List<string> { "sunny", "hot", "high", "false", "no" },
                new List<string> { "sunny", "hot", "high", "true", "no" },
                new List<string> { "overcast", "hot", "high", "false", "yes" },
                new List<string> { "rainy", "mild", "high", "false", "yes" },
                new List<string> { "rainy", "cool", "normal", "false", "yes" },
                new List<string> { "rainy", "cool", "normal", "true", "no" },
                new List<string> { "overcast", "cool", "normal", "true", "yes" },
                new List<string> { "sunny", "mild", "high", "false", "no" },
                new List<string> { "sunny", "cool", "normal", "false", "yes" },
                new List<string> { "rainy", "mild", "normal", "false", "yes" },
                new List<string> { "sunny", "mild", "normal", "true", "yes" },
                new List<string> { "overcast", "mild", "high", "true", "yes" },
                new List<string> { "overcast", "hot", "normal", "false", "yes" },
                new List<string> { "rainy", "mild", "high", "true", "no" }
            };
        }

        public IEnumerable<string> Attributes { get; }

        public IEnumerable<IList<string>> Records { get; }

        public string TargetAttribute => "Play";
    }
}
