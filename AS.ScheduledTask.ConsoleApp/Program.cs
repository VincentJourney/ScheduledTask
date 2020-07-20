using AS.Commom.Configuration;
using AS.Commom.Log;
using AS.QuartZ;
using AS.Service;
using System;
using System.Threading.Tasks;

namespace AS.ScheduledTask.ConsoleApp
{
    class Program
    {

        private static System.Timers.Timer time;

        private static bool OrderCancelFlag = true;//订单关闭
        private static bool OrderReturnFlag = ConfigurationUtil.OrderReturnEnabled;//订单退货
        private static bool RedisFlag = true;//订单退货


        private static DateTime Today = DateTime.Now.Date.AddHours(ConfigurationUtil.OrderReturnTime);

        private static DateTime TodayForRedis = DateTime.Now;

        static void Main(string[] args)
        {
            //await QuartzStartup.Start();

            time = new System.Timers.Timer();
            time.Enabled = true;
            time.Elapsed += OrderCancel;
         //   time.Elapsed += RedisSynchronization;

            if (ConfigurationUtil.OrderReturnEnabled)
                time.Elapsed += OrderReturn;

            time.Interval = Convert.ToInt32(1000 * ConfigurationUtil.CancelTriggerTime);
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
            try
            {
                if (OrderCancelFlag)
                {
                    OrderCancelFlag = false;
                    OrderService orderService = new OrderService();
                    orderService.OrderCancel();

                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    time.Start();
                    OrderCancelFlag = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("启用OrderCancel异常", ex);
            }
        }

        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OrderReturn(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now >= Today)
                {
                    OrderReturnFlag = true;
                    Today = Today.AddDays(1);
                }

                if (OrderReturnFlag)
                {
                    OrderReturnFlag = false;
                    OrderService orderService = new OrderService();
                    orderService.OrderRurn();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    time.Start();
                }
            }
            catch (Exception ex)
            {
                Log.Error("启用OrderReturn异常", ex);
            }
        }
        /// <summary>
        /// redis 同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RedisSynchronization(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now >= TodayForRedis)
                {
                    RedisFlag = true;
                    TodayForRedis = TodayForRedis.AddMinutes(ConfigurationUtil.RedisSyncTime);
                }

                if (RedisFlag)
                {
                    RedisFlag = false;
                    OrderService orderService = new OrderService();
                    orderService.RedisSynchronization();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    time.Start();
                }
            }
            catch (Exception ex)
            {
                Log.Error("启用RedisSynchronization异常", ex);
            }
        }
    }
}
