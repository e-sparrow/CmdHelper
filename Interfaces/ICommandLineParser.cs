using System.Collections.Generic;

namespace Birdhouse.Extended.CommandLine.Interfaces
{
    public interface ICommandLineParser
    {
        IDictionary<string, ICommandLineGroup> Parse(string value);
    }
}