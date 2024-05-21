using System.Collections.Generic;
using Birdhouse.Extended.CommandLine.Interfaces;

namespace Birdhouse.Extended.CommandLine
{
    public abstract class CommandLineParserBase
	    : ICommandLineParser
    {
        public CommandLineParserBase
        (
            char groupAppropriator,
            char groupSeparator
        )
        {
            _groupAppropriator = groupAppropriator;
            _groupSeparator = groupSeparator;
        }

        private readonly char _groupAppropriator;
        private readonly char _groupSeparator;
        
        protected abstract ICommandLineGroup ParseGroup(string value);
        
        public IDictionary<string, ICommandLineGroup> Parse(string value)
        {
            var groups = new Dictionary<string, ICommandLineGroup>();
            
            var groupInfos = new List<GroupInfo>();
            
            var lastWordBuffer = string.Empty;
            for (var index = 0; index < value.Length; index++)
            {
                if (value[index] == ' ')
                {
                    lastWordBuffer = string.Empty;
                    continue;
                }

                lastWordBuffer += value[index];

                if (value[index] == _groupAppropriator)
                {
                    var startIndex = index - lastWordBuffer.Length;
                    
                    var groupBuffer = string.Empty;
                    while (value[index] != _groupSeparator)
                    {
                        groupBuffer += value[index];
                        index++;
                    }

                    var group = ParseGroup(groupBuffer);
                    groups.Add(lastWordBuffer, group);

                    var groupLength = index - startIndex;
                    var groupInfo = new GroupInfo(startIndex, groupLength);
                    
                    groupInfos.Add(groupInfo);
                    
                    lastWordBuffer = string.Empty;
                }
            }

            var defaultGroupBuffer = string.Empty;
            for (var index = 0; index < value.Length; index++)
            {
                foreach (var group in groupInfos)
                {
                    while (index < group.StartIndex)
                    {
                        defaultGroupBuffer += value[index];
                        index++;
                    }

                    for (var passed = 0; passed < group.Length; passed++)
                    {
                        index++;
                    }
                }
                
                defaultGroupBuffer += value[index];
            }

            var defaultGroup = ParseGroup(defaultGroupBuffer);
            groups.Add(string.Empty, defaultGroup);

            return groups;
        }

        private readonly struct GroupInfo
        {
            public GroupInfo(int startIndex, int length)
            {
                StartIndex = startIndex;
                Length = length;
            }

            public int StartIndex
            {
                get;
            }

            public int Length
            {
                get;
            }
        }
    }
}