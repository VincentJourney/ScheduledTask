using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace AS.Commom.Configuration
{
    public static class ConfigurationUtil
    {
        static IConfiguration configuration;
        static ConfigurationUtil()
        {
            configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
        }

        public static T GetConfig<T>(string key)
        {
            try
            {
                return configuration.GetValue<T>(key);
            }
            catch (Exception ex)
            {
                throw new Exception($"【key:{key}】 | 【ex:{ex.Message}】");
            }
        }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string ConnectionString => GetConfig<string>("ConnectionString");
        public static string CRMUrl => GetConfig<string>("CRMUrl");
        public static string CRMTokenAppid => GetConfig<string>("CRMTokenAppid");
        public static string CRMTokenSecret => GetConfig<string>("CRMTokenSecret");

        /// <summary>
        /// 退货 每天定时执行的时间  CronExpression
        /// </summary>
        public static string CronExpression => GetConfig<string>("CronExpression");
        /// <summary>
        /// 是否启用过期礼券自动退货任务
        /// </summary>
        public static bool OrderReturnEnabled => GetConfig<bool>("OrderReturnEnabled");
        /// <summary>
        /// 待支付订单取消   每分钟执行
        /// </summary>
        public static int CancelTriggerTime => GetConfig<int>("CancelTriggerTime");
        /// <summary>
        /// 待支付逾期时间
        /// </summary>
        public static int ExpiryTime => GetConfig<int>("ExpiryTime");
        public static int OrderReturnTime => GetConfig<int>("OrderReturnTime");
        public static string RedisKey_ItemInventotyQty => "ItemInventotyQty";
        public static int RedisSyncTime => GetConfig<int>("RedisSyncTime");
    }
}
