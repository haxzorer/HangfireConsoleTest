using System.Threading;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace haxzorer.HangfireConsole
{
    class JobService : IJobService
    {
        public void RunJob(PerformContext context, IJobCancellationToken cancellationToken)
        {
            Thread.Sleep(5000);
            context.WriteLine("Hello, world 1");
            Thread.Sleep(5000);
            context.WriteLine("Hello, world 2");
        }
    }
}