using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using BLL.Exceptions;

namespace Middleware.Exception_Filtering
{
    class HttpResponesExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;
        public void OnActionExecuting(ActionExecutingContext context) { }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is BadRequestException exception400)
            {
                context.Result = new ObjectResult(exception400.Message)
                {
                    StatusCode = 400
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is UnauthorizedException exception401)
            {
                context.Result = new ObjectResult(exception401.Message)
                {
                    StatusCode = 401
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is NotFoundException exception404)
            {
                context.Result = new ObjectResult(exception404.Message)
                {
                    StatusCode = 404
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is ServerErrorException exception500)
            {
                context.Result = new ObjectResult(exception500.Message)
                {
                    StatusCode = 500
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is Exception exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = 500
                };
                context.ExceptionHandled = true;

            }
        }
    }
}
