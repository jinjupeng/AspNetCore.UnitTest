using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Filters
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                await HandleExceptionAsync(context, ex.Message);
            }
        }

        // 异常错误信息捕获，将错误信息用Json方式返回
        private async Task HandleExceptionAsync(HttpContext context, string msg)
        {
            var statusCode = context.Response.StatusCode;
            if (statusCode == 401)
            {
                msg = "未授权";
            }
            else if (statusCode == 404)
            {
                msg = "未找到服务";
            }
            else if (statusCode == 502)
            {
                msg = "请求错误";
            }
            else if (statusCode != 200)
            {
                msg = "未知错误";
            }
            var result = JsonConvert.SerializeObject(new { Msg = msg, Code = statusCode });

            context.Response.ContentType = "application/json;charset=utf-8";

            await context.Response.WriteAsync(result);
        }
    }

    // 扩展方法
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
