using System;

namespace Birdhouse.Extended.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RuntimeCommandLineInstructionAttribute
        : Attribute
    {
        public RuntimeCommandLineInstructionAttribute(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }
    }
}