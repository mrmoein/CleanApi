using CleanApi.Application.Common.Exceptions;
using CleanApi.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanApi.API.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(InternalServerErrorException), HandleInternalServerErrorException },
            { typeof(BadRequestException), HandleBadRequestException }
        };
    }


    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        ServiceResult<List<string>> details =
            ServiceResult.Failed(
                context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(),
                ServiceError.ValidationFormat);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        ServiceResult details = ServiceResult.Failed(ServiceError.DefaultError);

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };

        context.ExceptionHandled = true;
    }

    private static void HandleInternalServerErrorException(ExceptionContext context)
    {
        ServiceResult details = ServiceResult.Failed(ServiceError.InternalServerError(context.Exception.Message));

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        if (context.Exception is ValidationException exception)
        {
            ServiceResult<IDictionary<string, string[]>> details =
                ServiceResult.Failed(exception.Errors, ServiceError.Validation);

            context.Result = new BadRequestObjectResult(details);
        }

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        ServiceResult details =
            ServiceResult.Failed(new ServiceError(context.Exception.Message, ServiceError.NotFound.Code));

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        ServiceResult details = ServiceResult.Failed(ServiceError.ForbiddenError);

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status403Forbidden };

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        ServiceResult details = ServiceResult.Failed(ServiceError.ForbiddenError);

        context.Result = new UnauthorizedObjectResult(details);

        context.ExceptionHandled = true;
    }

    private static void HandleBadRequestException(ExceptionContext context)
    {
        ServiceResult details = ServiceResult.Failed(ServiceError.BadRequest(context.Exception.Message));

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
}