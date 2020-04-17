using AS.Commom.Configuration;
using AS.QuartZ;
using AS.Service;
using System;
using System.Threading.Tasks;

namespace AS.ScheduledTask.ConsoleApp
{
    class Program
    {

        private static System.Timers.Timer time;
        private static bool OrderCancelFlag = true;
        static void Main(string[] args)
        {
            //await QuartzStartup.Start();

            time = new System.Timers.Timer();
            time.Enabled = true;
            time.Elapsed += OrderCancel;
            time.Elapsed += OrderReturn;
            time.Start();

            Console.ReadKey();

        }
        /// <summary>
        /// 订单取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OrderCancel(object sender, EventArgs e)
        {
            if (OrderCancelFlag)
            {
                OrderCancelFlag = false;
                OrderService orderService = new OrderService();
                orderService.OrderCancel();
                time.Interval = Convert.ToInt32(1000 * ConfigurationUtil.CancelTriggerTime);
                GC.WaitForPendingFinalizers();
                GC.Collect();
                time.Start();
                OrderCancelFlag = true;
            }
        }

        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OrderReturn(object sender, EventArgs e)
        {

        }
    }
}
