using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace haxzorer.HangfireConsole
{
    public class HangfireAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return true;
        }
    }
}