using System.Collections.Generic;

namespace C45.Loaders
{
    public interface IDataFile
    {
        IEnumerable<string> Attributes { get; }

        IEnumerable<IList<string>> Records { get; }

        string TargetAttribute { get; }
    }
}
