using System;

namespace Birdhouse.Extended.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class BuildCommandLineParserAttribute 
        : Attribute
    {
        public BuildCommandLineParserAttribute(Type type)
        {
            Type = type;
        }

        public Type Type
        {
            get;
        }
    }
}
