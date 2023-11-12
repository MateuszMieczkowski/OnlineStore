using Quartz;

namespace OnlineStore.Server.Jobs
{
    public static class JobsDiExtensions
    {
        public static IServiceCollection RegisterQuartzJobs(this IServiceCollection services)
        {
            services.AddTransient<EmailSenderJob>();

            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("EmailSenderJob");
                q.AddJob<EmailSenderJob>(opts =>
                {
                    opts.WithIdentity(jobKey);
                    opts.DisallowConcurrentExecution();
                });

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("EmailSenderJob-trigger")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(10)
                        .RepeatForever()));
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
            
            return services;
        }
    }
}
