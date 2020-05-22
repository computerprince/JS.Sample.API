using JS.Sample.Common.Models;
using JS.Sample.Infratructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;


namespace JS.Sample.Infratructure.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private ILogger<ApiExceptionFilter> _Logger;

        /// <summary>
        /// Dependency to be injected and resolved
        /// </summary>
        /// <param name="logger"></param>
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _Logger = logger;
        }


        /// <summary>
        /// This method will be called on the exceptions which are unhandled from the code
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            CommonResponse apiError = null;
            if (context.Exception is ApiException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new CommonResponse(ex.Message, false);
                apiError.Errors = ex.Errors;

                context.HttpContext.Response.StatusCode = ex.StatusCode;

                _Logger.LogWarning($"Application thrown error: {ex.Message}", ex);
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new CommonResponse("Unauthorized Access", false);
                context.HttpContext.Response.StatusCode = 401;
                _Logger.LogWarning("Unauthorized Access in Controller Filter.");
            }
            else
            {
                // Unhandled errors
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;


                apiError = new CommonResponse(msg, false);
                apiError.Details = stack;

                context.HttpContext.Response.StatusCode = 500;

                // handle logging here
                _Logger.LogError(new EventId(0), context.Exception, msg);
            }

            // always return a JSON result
            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}
