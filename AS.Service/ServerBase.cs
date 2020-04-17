using AICFrame.Common.Redis;
using AS.Commom.Log;
using AS.OCR.Dapper.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AS.Service
{
    public class ServerBase
    {
        protected static RedisHelper rh { get; set; }
        protected RedisHelper GetRedisHelper()
        {
            if (rh == null)
            {

                Stopwatch sw = new Stopwatch();
                sw.Start();
                rh = new RedisHelper();
                sw.Stop();
                TimeSpan ts2 = sw.Elapsed;
                if (ts2.TotalMilliseconds > 500)
                {
                    Log.Info(string.Format("获取redis总共花费{0}ms.", ts2.TotalMilliseconds));
                }

            }


            return rh;
        }

    }
}
