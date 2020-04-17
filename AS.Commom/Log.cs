using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AS.Commom.Log
{
    public enum LogLevel
    {
        Debug,
        Info,
        Error
    }

    public class Log
    {
        private static LogLevel _level = LogLevel.Debug;

        public static Action<LogLevel, string, Exception> LogAction = (level, mes, ex) =>
          {
              if (level >= _level)
              {
                  string directPath = level == LogLevel.Error ? $"{AppContext.BaseDirectory}Log_Error/" : $"{AppContext.BaseDirectory}Log_Info/";
                  string logfile = $"{directPath}{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                  if (!Directory.Exists(directPath))
                      Directory.CreateDirectory(directPath);

                  StreamWriter sw = File.AppendText(logfile);
                  var exInfo = ex == null ? "" : $@"异常信息:{ex?.Message}
异常堆栈: { ex?.StackTrace}";
                  var loginfo = $@"
时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
类型:{level}
基本信息:{mes}
{exInfo}";
                  sw.WriteLine(loginfo);
                  sw.Close();
              }
          };

        public static void Error(string Message, Exception ex) => LogAction(LogLevel.Error, Message, ex);
        public static void Info(string Message) => LogAction(LogLevel.Info, Message, null);
    }
}