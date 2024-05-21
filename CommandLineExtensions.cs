using System.Linq;
using Birdhouse.Extended.CommandLine.Interfaces;

namespace Birdhouse.Extended.CommandLine
{
    public static class CommandLineExtensions
    {
        public static bool HasOption(this ICommandLineGroup self, string name)
        {
            var result = self.Options.Any(value => value == name);
            return result;
        }
    }
}