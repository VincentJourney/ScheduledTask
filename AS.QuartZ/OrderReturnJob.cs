using AS.Commom.Log;
using AS.Service;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace AS.QuartZ
{
    public class OrderReturnJob : IJob
    {
        private readonly OrderService orderService = new OrderService();
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Log.Info($"===={DateTime.Now.ToString()}：开始执行自动退货====");
                orderService.OrderRurn();
            });
        }

    }
}
