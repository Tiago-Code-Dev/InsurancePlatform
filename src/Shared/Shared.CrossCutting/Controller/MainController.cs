using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using Shared.CrossCutting.Notifications;
using Shared.CrossCutting.Response;

namespace Shared.CrossCutting.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class MainController : ControllerBase
{
    private readonly INotifier _notifier;

    protected MainController(INotifier notifier) =>
        _notifier = notifier;

    protected bool IsOperationValid()
    {
        return !_notifier.HasNotification();
    }

    protected void NotifyModelStateErrors(ModelStateDictionary modelState)
    {
        foreach (var kvp in modelState)
        {
            var field = kvp.Key;
            var errors = kvp.Value.Errors;

            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                _notifier.Handle(new Notification(field, errorMsg));

                Log.Warning("Model validation error: {ErrorMessage} (Path: {Path}, TraceId: {TraceId})",
                    errorMsg, HttpContext?.Request?.Path, HttpContext?.TraceIdentifier);
            }
        }
    }

    protected IActionResult CustomResponse(object? result = null)
    {
        if (result is ICustomResponse custom)
        {
            return StatusCode(custom.StatusCode, result);
        }

        if (IsOperationValid())
        {
            Log.Information("Request successful. Path: {Path}, TraceId: {TraceId}",
               HttpContext?.Request?.Path, HttpContext?.TraceIdentifier);

            return Ok(CustomResponse<object>.Ok(result));
        }

        var messages = _notifier.GetNotifications()
            .Select(n =>
                string.IsNullOrEmpty(n.Field)
                    ? new MessageResponse(null, n.Message)
                    : new MessageResponse(n.Field, n.Message)
            ).ToList();

        Log.Warning("Request failed validation. Path: {Path}, TraceId: {TraceId}",
            HttpContext?.Request?.Path, HttpContext?.TraceIdentifier);
        return BadRequest(CustomResponse<object>.Fail(messages));
    }

    protected void NotifyError(string message) {
       _notifier.Handle(new Notification(message));
        Log.Warning("Custom error notified: {Message}, Path: {Path}, TraceId: {TraceId}",
            message, HttpContext?.Request?.Path, HttpContext?.TraceIdentifier);
    }

    protected IActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            NotifyModelStateErrors(modelState);

        return CustomResponse();
    }
}
