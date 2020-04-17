using AS.Commom.Log;
using AS.Service;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace AS.QuartZ
{
    public class OrderCancelJob : IJob
    {
        private readonly OrderService orderService = new OrderService();
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
               Console.WriteLine($"{DateTime.Now.ToString()}：开始执行逾期未支付订单自动关闭");
                Log.Info($"{DateTime.Now.ToString()}：开始执行逾期未支付订单自动关闭");
                orderService.OrderCancel();
            });
        }

    }
}
