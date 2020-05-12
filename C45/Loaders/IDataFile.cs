using System.Collections.Generic;

namespace C45.Loaders
{
    public interface IDataFile
    {
        IList<string> Attributes { get; }

        IEnumerable<IList<string>> Records { get; }

        string ClassificationAttribute { get; }
    }
}
