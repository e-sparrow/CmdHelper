#if UNITY_EDITOR
using Birdhouse.Common.Builds.Unity;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Birdhouse.Extended.CommandLine
{
    public class UnityBuildExecutor
        : UnityBuildExecutorBase
    {
        public UnityBuildExecutor(BuildPlayerOptions? options = null)
        {
            options ??= CreateDefaultOptions();
            
            _options = options.Value;
        }

        private readonly BuildPlayerOptions _options;

        private static BuildPlayerOptions CreateDefaultOptions()
        {
            var result = UnityBuildHelper.DefaultOptionsProvider.GetValue(false, default);
            return result;
        }
        
        protected override BuildPlayerOptions GetOptions()
        {
            return _options;
        }

        protected override BuildReport GetReportFromOptions(BuildPlayerOptions options)
        {
            var result = BuildPipeline.BuildPlayer(options);
            return result;
        }
    }
}
#endif
