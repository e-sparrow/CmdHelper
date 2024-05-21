using System;
using Birdhouse.Features.Executions.Interfaces;
using Birdhouse.Features.Aggregators.Interfaces;

namespace Birdhouse.Extended.CommandLine.Interfaces
{
    public interface IBuildExecutor<TOptions, out TReport>
        : IExecutor
    {
        event Action OnPreprocess;
        event Action<TReport> OnGetReport;

        IAggregator<TOptions> OptionsAggregator
        {
            get;
        }

        TOptions DefaultOptions
        {
            get;
        }
    }
}