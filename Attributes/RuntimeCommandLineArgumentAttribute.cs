using System;

namespace Birdhouse.Extended.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RuntimeCommandLineArgumentAttribute
        : Attribute
    {
        public RuntimeCommandLineArgumentAttribute(string name, bool executeDefault = false, string defaultValue = null)
        {
            Name = name;
            ExecuteDefault = executeDefault;
            DefaultValue = defaultValue;
        }

        public string Name
        {
            get;
        }

        public bool ExecuteDefault
        {
            get;
        }

        public string DefaultValue
        {
            get;
        }
    }
}