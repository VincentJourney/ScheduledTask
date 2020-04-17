using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AS.ScheduledTask.Middleware
{
    /// <summary>
    /// 全局异常
    /// </summary>
    public class CustomExceptionHandlerMiddleware
    {
        private RequestDelegate _next { get; }

        private ILogger _log { get; }

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _log = loggerFactory.CreateLogger(typeof(CustomExceptionHandlerMiddleware));
        }

        public async Task Invoke(HttpContext contexts)
        {
            try
            {
                await _next(contexts);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(contexts, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            var mes = $@"【异常信息 ：{e.Message}】
                         【堆栈信息 ：{e.StackTrace}】";
            _log.LogError(mes);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response
                .WriteAsync(e.Message)
                .ConfigureAwait(false);
        }
    }
}
