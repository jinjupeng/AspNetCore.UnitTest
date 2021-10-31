using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AspNetCore.UnitTest.Api.Filters
{
    /// <summary>
    /// 自定义全局异常过滤器：当程序发生异常时，处理系统出现的未捕获的异常
    /// 自定义一个全局异常过滤器需要实现IExceptionFilter接口
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="env"></param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// IExceptionFilter接口会要求实现OnException方法，当系统发生未捕获异常时就会触发这个方法。
        /// OnException方法有一个ExceptionContext异常上下文，其中包含了具体的异常信息，HttpContext及mvc路由信息。
        /// 系统一旦出现未捕获异常后，比较常见的做法就是使用日志工具，将异常的详细信息记录下来，方便调试
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            // 如果异常没有被处理则进行处理
            if (context.ExceptionHandled == false)
            {
                var exception = context.Exception;

                //写入日志
                _logger.LogError(context.HttpContext.Request.Path, context.Exception);

                context.Result = new ContentResult
                {
                    // 返回状态码设置为200，表示成功
                    StatusCode = StatusCodes.Status200OK,
                    // 设置返回格式
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(new { Msg = exception.Message, Code = 200 })
                };
            }
            // 设置为true，表示异常已经被处理了
            context.ExceptionHandled = true;
        }

    }
}
