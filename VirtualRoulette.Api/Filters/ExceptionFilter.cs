using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualRoulette.Service;

namespace VirtualRoulette.Api.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var baseException = context.Exception.GetBaseException();

            int statusCode = StatusCodes.Status500InternalServerError;

            BaseApiException baseApiException = null;
            if (baseException is BaseApiException)
            {
                baseApiException = ((BaseApiException)baseException);
                statusCode = baseApiException.ResponseHttpStatusCode;
            }

            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new JsonResult(new
            {
                StatusCode = statusCode,
                error = baseException.Message,
                error_description = baseApiException?.BackEndMessage
            });

            base.OnException(context);
        }
    }
}
