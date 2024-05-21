using System;

namespace Birdhouse.Extended.CommandLine.Attributes
{
    /// <summary>
    /// Attribute used to invoke some methods from command line or set boolean fields/properties to true
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public class BuildCommandLineInstructionAttribute
        : Attribute
    {
        public BuildCommandLineInstructionAttribute(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }
    }
}
