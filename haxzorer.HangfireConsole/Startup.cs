using Hangfire;
using Hangfire.Console;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace haxzorer.HangfireConsole
{
    public class Startup
    {
        public static ConnectionMultiplexer Redis { get; set; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var redisConnectionString = "localhost:6379";

            Redis = ConnectionMultiplexer.Connect(redisConnectionString);

            services.AddHangfire(configuration => configuration
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseSerilogLogProvider()
             .UseConsole()
             .UseRedisStorage(Redis,
                 new Hangfire.Redis.RedisStorageOptions
                 {
                     Prefix = "Test"
                 }
             ));


            services.AddScoped<IJobService, JobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthFilter() },
                IgnoreAntiforgeryToken = true
            });

            // Schedule jobs
            JobScheduler.ScheduleJobs(recurringJobManager);
        }
    }
}
