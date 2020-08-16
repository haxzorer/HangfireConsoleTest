using Hangfire;
using Hangfire.Server;

namespace haxzorer.HangfireConsole
{
    interface IJobService
    {
        void RunJob(PerformContext context, IJobCancellationToken cancellationToken);
    }
}