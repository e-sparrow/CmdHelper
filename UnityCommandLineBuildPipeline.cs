#if UNITY_EDITOR
using System;
using Birdhouse.Extended.CommandLine;
using Birdhouse.Extended.CommandLine.Interfaces;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Birdhouse.CommandLine
{
    public static class UnityCommandLineBuildPipeline
    {
        public static ICommandLineExecutor CommandLineExecutor => LazyExecutor.Value;
        public static IBuildExecutor<BuildPlayerOptions, BuildReport> BuildExecutor => LazyProcessor.Value;
        
        private static readonly Lazy<ICommandLineExecutor> LazyExecutor
            = new Lazy<ICommandLineExecutor>(CreateExecutor);

        private static readonly Lazy<IBuildExecutor<BuildPlayerOptions, BuildReport>> LazyProcessor 
            = new Lazy<IBuildExecutor<BuildPlayerOptions, BuildReport>>(CreateProcessor);
        
        public static void Build()
        {
            LazyExecutor.Value.Execute();
            LazyProcessor.Value.Execute();
        }

        private static ICommandLineExecutor CreateExecutor()
        {
            var result = new BuildReflectionCommandLineExecutor();
            return result;
        }

        private static IBuildExecutor<BuildPlayerOptions, BuildReport> CreateProcessor()
        {
            var result = new UnityBuildExecutor();
            return result;
        }
    }
}
#endif