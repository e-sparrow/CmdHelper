using System;
using Birdhouse.CommandLine;

namespace Birdhouse.Extended.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public class BuildCommandLineArgumentAttribute 
        : Attribute
    {
        public BuildCommandLineArgumentAttribute
        (
            string name
        )
        {
            Name = name;

            Appropriator = CommandLineConstants.InGroupAppropriator;
            Separator = CommandLineConstants.InGroupSeparator;
        }

        public BuildCommandLineArgumentAttribute
        (
            string name,
            char appropriator,
            char separator
        )
        {
            Name = name;
            
            Appropriator = appropriator;
            Separator = separator;
        }

        public string Name
        {
            get;
        }

        public char Appropriator
        {
            get;
        }
        
        public char Separator
        {
            get;
        }
    }
}
