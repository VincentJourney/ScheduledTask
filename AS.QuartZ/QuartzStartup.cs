using AS.Commom.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AS.QuartZ
{
    public static class QuartzStartup
    {
        public static async Task<string> Start()
        {
            //1、声明一个调度工厂
            var _schedulerFactory = new StdSchedulerFactory();
            //2、通过调度工厂获得调度器
            var _scheduler = await _schedulerFactory.GetScheduler();
            //3、开启调度器
            await _scheduler.Start();

            //4、触发器   5、任务
            #region 退货  每天执行
            var ReturnTrigger = TriggerBuilder.Create()
                             .WithSchedule(CronScheduleBuilder.CronSchedule(new CronExpression(ConfigurationUtil.CronExpression)))
                            .Build();

            var ReturnJobDetail = JobBuilder.Create<OrderReturnJob>()
                            .WithIdentity("Return", "group")
                            .Build();
            #endregion

            #region 待支付订单取消   每分钟执行
            var CancelTrigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(ConfigurationUtil.CancelTriggerTime)
                            .RepeatForever())//每两分钟执行一次
                            .Build();

            var CancelJobDetail = JobBuilder.Create<OrderCancelJob>()
                            .WithIdentity("Cancel", "group")
                            .Build();
            #endregion

            //6、将触发器和任务器绑定到调度器中

            if (ConfigurationUtil.OrderReturnEnabled)
                await _scheduler.ScheduleJob(ReturnJobDetail, ReturnTrigger);

            await _scheduler.ScheduleJob(CancelJobDetail, CancelTrigger);
            return await Task.FromResult("将触发器和任务器绑定到调度器中完成");
        }
    }
}
