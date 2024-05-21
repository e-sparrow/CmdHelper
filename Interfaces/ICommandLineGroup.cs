using System.Collections.Generic;

namespace Birdhouse.Extended.CommandLine.Interfaces
{
    public interface ICommandLineGroup
    {
        IDictionary<string, string> Variables
        {
            get;
        }

        IEnumerable<string> Options
        {
            get;
        }
    }
}