using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Shared.Responses;
using FIAP.AquaGuard.Exception;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FIAP.AquaGuard.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var payload = new PayloadResponse<object>
            {
                Status = nameof(ResponseStatus.Error),
                Message = context.Exception.Message,
                Data = null
            };

            switch (context.Exception)
            {
                case ErrorOnValidationException:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new BadRequestObjectResult(payload);
                    break;
                case LoginException:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Result = new UnauthorizedObjectResult(payload);
                    break;
                default:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Result = new ObjectResult(new PayloadResponse<object>
                    {
                        Status = nameof(ResponseStatus.Error),
                        Message = ResourceMessagesException.UNKNOWN_ERROR,
                        Data = null
                    });
                    break;
            }

            context.ExceptionHandled = true;
        }
    }
}
