using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Birdhouse.Abstractions.Parsers;
using Birdhouse.Common.Extensions;
using Birdhouse.Common.Reflection.MutableMembers;
using Birdhouse.Extended.CommandLine.Attributes;
using Birdhouse.Extended.CommandLine.Interfaces;
using Birdhouse.Features.Registries;
using Birdhouse.Features.Registries.Interfaces;
using UnityEngine;

namespace Birdhouse.Extended.CommandLine
{
    public class BuildReflectionCommandLineExecutor
        : ICommandLineExecutor
    {
        private readonly IRegistryDictionary<Type, IParser<string, object>> _parsers 
            = new RegistryDictionary<Type, IParser<string, object>>();
        
        public void Execute()
        {
            var groupTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.HasCustomAttribute<BuildCommandLineGroupAttribute>());

            var groups = new CommandLineParser()
                .Parse(Environment.CommandLine);

            foreach (var group in groupTypes)
            {
                var exemplar = CreateExemplar();

                var groupAttribute = group.GetCustomAttribute<BuildCommandLineGroupAttribute>();
                var groupName = groupAttribute.Name;

                var isValidGroup = groups.TryGetValue(groupName, out var commandLineGroup);
                if (!isValidGroup)
                {
                    var invalidGroupMessage = $"Can't parse group with name \"{groupName}\"";
                    Debug.LogWarning(invalidGroupMessage);

                    continue;
                }

                ParseArguments(group);
                ParseOptions(group);
                ExecutePostprocessors(group);

                object CreateExemplar()
                {
                    var isStatic = group.IsStatic();
                    if (isStatic)
                    {
                        return null;
                    }

                    var isValid = group.TryGetEmptyConstructor(out var constructor);
                    if (isValid)
                    {
                        var result = constructor.Invoke(Array.Empty<object>());
                        return result;
                    }

                    var invalidClassMessage = $"Make class \"{group}\" static or add here constructor without arguments";
                    throw new ArgumentException(invalidClassMessage);
                }

                void ParseArguments(Type groupType)
                {
                    var parserTokens = new List<IDisposable>();

                    var parserMembers = groupType
                        .GetMembers()
                        .Where(member => member.HasCustomAttribute<BuildCommandLineParserAttribute>());

                    foreach (var parserMember in parserMembers)
                    {
                        var parserAttribute = parserMember.GetCustomAttribute<BuildCommandLineParserAttribute>();

                        IParser<string, object> parser;
                        var isReadable = parserMember.TryCreateReadable(out var readable, out var type);
                        if (isReadable)
                        {
                            var isValid = typeof(IParser<string, object>).IsAssignableFrom(type);
                            if (isValid)
                            {
                                parser = (IParser<string, object>) readable.GetValue(parserMember);
                            }
                            else
                            {
                                throw new ArgumentException($"Can't cast parser from type \"{type.Name}\" to parser interface!");
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Member with name \"{parserMember.Name}\" is not readable!");
                        }

                        var canAddParser = !_parsers.ContainsKey(parserAttribute.Type);
                        if (canAddParser)
                        {
                            var parserToken = _parsers.Register(parserAttribute.Type, parser);
                            parserTokens.Add(parserToken);
                        }
                    }

                    var members = groupType
                        .GetMembers()
                        .Where(field => field.HasCustomAttribute<BuildCommandLineArgumentAttribute>());

                    foreach (var member in members)
                    {
                        var isWritable = member.TryCreateWritable(out var writable, out var type);
                        if (isWritable)
                        {
                            var argumentAttribute = member.GetCustomAttribute<BuildCommandLineArgumentAttribute>();
                            var argumentName = argumentAttribute.Name;
                            
                            var isParsed = TryParseValue(commandLineGroup, type, argumentName, out var parsedValue);
                            if (isParsed)
                            {
                                writable.SetValue(exemplar, parsedValue);
                            }
                            else
                            {
                                throw new ArgumentException($"Unable to parse value \"{parsedValue}\" to type {type.Name}!");
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Member with name \"{member.Name}\" is not writable!");
                        }
                    }
                    
                    foreach (var parserToken in parserTokens)
                    {
                        parserToken.Dispose();
                    }
                }

                void ParseOptions(Type group)
                {
                    var members = group
                        .GetMembers()
                        .Where(field => field.HasCustomAttribute<BuildCommandLineInstructionAttribute>());

                    foreach (var member in members)
                    {
                        var isWritable = member.TryCreateWritable(out var writable, out var type);
                        if (isWritable)
                        {
                            var isValid = type == typeof(bool);
                            if (isValid)
                            {
                                var instructionAttribute = member.GetCustomAttribute<BuildCommandLineInstructionAttribute>();
                                var argumentName = instructionAttribute.Name;

                                var hasValue = commandLineGroup.HasOption(argumentName);
                                writable.SetValue(exemplar, hasValue);
                            }
                            else
                            {
                                throw new ArgumentException($"Can't use member with name \"{member.Name}\" as instruction! This member should have a boolean type.");
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Member with name \"{member.Name}\\\" is not writable!");
                        }
                    }
                }

                void ExecutePostprocessors(Type group)
                {
                    var dictionary = new Dictionary<BuildCommandLinePostprocessorAttribute, Action>();

                    var methods = group
                        .GetMethods()
                        .Where(method => method.HasCustomAttribute<BuildCommandLinePostprocessorAttribute>());

                    foreach (var method in methods)
                    {
                        var attribute = method.GetCustomAttribute<BuildCommandLinePostprocessorAttribute>();

                        if (method.GetParameters().Length != 0)
                        {
                            var invalidParametersLength = $"Can't use method with name \"{method.Name}\" as post-processor!";
                            throw new ArgumentException(invalidParametersLength);
                        }

                        dictionary.Add(attribute, Invoke);

                        void Invoke()
                        {
                            var parameters = Array.Empty<object>();
                            method.Invoke(exemplar, parameters);
                        }
                    }

                    var postprocessors = dictionary
                        .OrderBy(value => value.Key.Order)
                        .Select(value => value.Value);

                    foreach (var postprocessor in postprocessors)
                    {
                        postprocessor.Invoke();
                    }
                }
            }
        }
        
        private bool TryParseValue(ICommandLineGroup group, Type type, string name, out object result)
        {
            result = null;
            
            var hasValue = group.Variables.TryGetValue(name, out var value);
            if (hasValue)
            {
                var canParse = _parsers.TryGetValue(type, out var parser);
                if (canParse)
                {
                    var isParsed = parser.TryParse(value, out result);
                    return isParsed;
                }
            }

            return false;
        }
    }
}