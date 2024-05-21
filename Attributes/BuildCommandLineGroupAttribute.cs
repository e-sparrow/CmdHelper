using System;

namespace Birdhouse.Extended.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class BuildCommandLineGroupAttribute 
        : Attribute
    {
        public BuildCommandLineGroupAttribute(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }
    }
}