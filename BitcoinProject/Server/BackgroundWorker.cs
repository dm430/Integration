using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Server
{
    public static class BackgroundWorker
    {
        private static IScheduler _scheduler;

        public static void StartScheduler()
        {
            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.Start();
        }

        public static void ShutdownScheduler()
        {
            _scheduler.Shutdown();
        }

        public static void ScheduleChecker(List<byte[]> ipAddresses)
        {
            // Create the background job
            IJobDetail job = JobBuilder.Create<CheckConnection>()
                .WithIdentity("CheckConnection", "Job")
                .Build();

            // Pass data to the job
            job.JobDataMap["ipAddresses"] = ipAddresses;

            // Create trigger at specific time interval
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("CheckConnectionTrigger", "Trigger")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Constants.Hour, Constants.Minute))
                .Build();

            // Match the job and trigger
            _scheduler.ScheduleJob(job, trigger);
        }
    }

    public class CheckConnection : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            // Get data passed from outside
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            var ipAddresses = (List<byte[]>) dataMap["ipAddresses"];

            // List of dead IPs
            var removedAddresses = new List<byte[]>();

            // Empty list?
            if (ipAddresses.Count == 0)
            {
                // Do nothing
                return;
            }

            foreach (var ipAddress in ipAddresses)
            {
                var ip = new IPAddress(ipAddress);

                // Ping the address
                var ping = new Ping();
                PingReply reply = ping.Send(ip);

                // Un-reachable?
                if (reply != null && reply.Status != IPStatus.Success)
                {
                    // Consider it dead
                    removedAddresses.Add(ipAddress);
                }
            }

            // Remove un-reachable address
            foreach (var removedAddress in removedAddresses)
            {
                ipAddresses.Remove(removedAddress);
            }
        }
    }
}
