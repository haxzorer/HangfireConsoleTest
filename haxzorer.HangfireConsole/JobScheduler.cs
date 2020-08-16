using Hangfire;

namespace haxzorer.HangfireConsole
{
    public static class JobScheduler
    {
        public static void ScheduleJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate<IJobService>("TestJob", x => x.RunJob(null, null), Cron.Never);
        }
    }
}