using Quartz;
using Quartz.Impl;

namespace Controllers.Jobs
{
    public class SmsJob
    {
        public static void Start()
        {
            Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
            
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            
            IJobDetail job = JobBuilder.Create<SendNegotioationSmsJob>()
                .WithIdentity("job1", "group1")
                .Build();

            
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .Build();
            scheduler.ScheduleJob(job, trigger);
            // and start it off
            scheduler.Start();

        }
    }
}
