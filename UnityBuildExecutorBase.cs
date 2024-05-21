using System;
using Birdhouse.Extended.CommandLine.Interfaces;
using Birdhouse.Features.Aggregators;
using Birdhouse.Features.Aggregators.Interfaces;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Birdhouse.Extended.CommandLine
{
    public abstract class UnityBuildExecutorBase
        : IBuildExecutor<BuildPlayerOptions, BuildReport>
    {
        protected UnityBuildExecutorBase()
        {
            _lazyValue = new Lazy<BuildPlayerOptions>(GetOptions);
        }
        
        public event Action OnPreprocess = () => { };
        public event Action<BuildReport> OnGetReport = _ => { };
        
        public IAggregator<BuildPlayerOptions> OptionsAggregator
        {
            get;
        } = new Aggregator<BuildPlayerOptions>();

        private readonly Lazy<BuildPlayerOptions> _lazyValue;

        public BuildPlayerOptions DefaultOptions => _lazyValue.Value;

        protected abstract BuildPlayerOptions GetOptions();
        protected abstract BuildReport GetReportFromOptions(BuildPlayerOptions options);
        
        public void Execute()
        {
            OnPreprocess.Invoke();

            var options = _lazyValue.Value;
            options = OptionsAggregator.Process(options);

            var report = GetReportFromOptions(options);
            OnGetReport.Invoke(report);
        }
    }
}