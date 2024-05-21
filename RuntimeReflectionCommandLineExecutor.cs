using System;
using System.Linq;
using System.Reflection;
using Birdhouse.Common.Extensions;
using Birdhouse.Extended.CommandLine.Attributes;
using Birdhouse.Extended.CommandLine.Interfaces;

namespace Birdhouse.Extended.CommandLine
{
    public sealed class RuntimeReflectionCommandLineExecutor
        : ICommandLineExecutor
    {
        public void Execute()
        {
            var groupTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.HasCustomAttribute<RuntimeCommandLineGroupAttribute>());

            var groups = new CommandLineParser()
                .Parse(Environment.CommandLine);

            foreach (var group in groupTypes)
            {
                var groupAttribute = group.GetCustomAttribute<RuntimeCommandLineGroupAttribute>();
                var groupName = groupAttribute.Name;

                var isValidGroup = groups.TryGetValue(groupName, out var commandLineGroup);
                if (isValidGroup)
                {
                    var methods = group
                        .GetMethods()
                        .Where(method => method.IsStatic);

                    foreach (var method in methods)
                    {
                        var isInstruction = method.TryGetCustomAttribute<RuntimeCommandLineInstructionAttribute>(out var instructionAttribute);
                        if (isInstruction)
                        {
                            var hasInstruction = commandLineGroup.Options.Contains(instructionAttribute.Name);
                            if (hasInstruction)
                            {
                                method.Invoke(null, Array.Empty<object>());
                            }
                        }
                        
                        var isArgument = method.TryGetCustomAttribute<RuntimeCommandLineArgumentAttribute>(out var argumentAttribute);
                        if (isArgument)
                        {
                            var hasArgument = commandLineGroup.Variables.TryGetValue(argumentAttribute.Name, out var argument);
                            if (hasArgument)
                            {
                                method.Invoke(null, argument.AsSingleArray<object>());
                            }
                            else if (argumentAttribute.ExecuteDefault)
                            {
                                method.Invoke(null, argumentAttribute.DefaultValue.AsSingleArray<object>());
                            }
                        }
                    }
                }
            }
        }
    }
}