using System;

namespace Birdhouse.Extended.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RuntimeCommandLineGroupAttribute
        : Attribute
    {
        public RuntimeCommandLineGroupAttribute(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }
    }
}