using System;
using System.Collections.Generic;
using Birdhouse.Extended.CommandLine.Interfaces;

namespace Birdhouse.Extended.CommandLine
{
    public static class CommandLineHelper
    {
        private static readonly Lazy<IDictionary<string, ICommandLineGroup>> LazyGroups =
            new Lazy<IDictionary<string, ICommandLineGroup>>(CreateGroups);

        public static IDictionary<string, ICommandLineGroup> Groups => LazyGroups.Value;

        private static IDictionary<string, ICommandLineGroup> CreateGroups()
        {
            var parser = new CommandLineParser();
            
            var result = parser.Parse(Environment.CommandLine);
            return result;
        }
    }
}