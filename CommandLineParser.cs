using System.Collections.Generic;
using Birdhouse.CommandLine;
using Birdhouse.Extended.CommandLine.Interfaces;

namespace Birdhouse.Extended.CommandLine
{
    public sealed class CommandLineParser
        : CommandLineParserBase
    {
        public CommandLineParser
        (
            char groupAppropriator = CommandLineConstants.GroupAppropriator, 
            char groupSeparator = CommandLineConstants.GroupSeparator, 
            char inGroupAppropriator = CommandLineConstants.InGroupAppropriator, 
            char inGroupSeparator = CommandLineConstants.InGroupSeparator
        ) : base(groupAppropriator, groupSeparator)
        {
            _inGroupAppropriator = inGroupAppropriator;
            _inGroupSeparator = inGroupSeparator;
        }

        private readonly char _inGroupAppropriator;
        private readonly char _inGroupSeparator;
        
        protected override ICommandLineGroup ParseGroup(string value)
        {
            var variables = new Dictionary<string, string>();
            var options = new List<string>();
            
            var lastWordBuffer = string.Empty;
            for (var index = 0; index < value.Length; index++)
            {
                lastWordBuffer += value[index];

                if (value[index] == _inGroupSeparator)
                {
                    options.Add(lastWordBuffer);
                    lastWordBuffer = string.Empty;
                }
                
                if (value[index] == _inGroupAppropriator)
                {
                    var valueBuffer = string.Empty;
                    while (value[index] != _inGroupSeparator)
                    {
                        if (value[index] == ' ')
                        {
                            index++;
                            continue;
                        }
                        
                        valueBuffer += value[index];
                        index++;
                    }

                    variables.Add(lastWordBuffer, valueBuffer);
                    lastWordBuffer = string.Empty;
                }
            }

            var result = new CommandLineGroup(variables, options);
            return result;
        }
        
        private sealed class CommandLineGroup 
            : ICommandLineGroup
        {
            public CommandLineGroup(IDictionary<string, string> variables, IEnumerable<string> options)
            {
                Variables = variables;
                Options = options;
            }


            public IDictionary<string, string> Variables
            {
                get;
            }

            public IEnumerable<string> Options
            {
                get;
            }
        }
    }
}