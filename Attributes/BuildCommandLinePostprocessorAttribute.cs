using System;

namespace Birdhouse.Extended.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BuildCommandLinePostprocessorAttribute
        : Attribute
    {
        public BuildCommandLinePostprocessorAttribute(int order = 0)
        {
            Order = order;
        }

        public int Order
        {
            get;
            private set;
        }
    }
}
