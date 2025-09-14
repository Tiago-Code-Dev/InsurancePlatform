using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.CrossCutting.Notifications;
using Shared.CrossCutting.Response;

namespace Shared.CrossCutting.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class MainController : ControllerBase
{
    private readonly INotifier _notifier;

    protected MainController(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected bool IsOperationValid()
    {
        return !_notifier.HasNotification();
    }

    protected void NotifyModelStateErrors(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);
        foreach (var error in errors)
        {
            var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
            _notifier.Handle(new Notification(errorMsg));
        }
    }

    protected IActionResult CustomResponse(object result = null)
    {
        if (IsOperationValid())
        {
            return Ok(new CustomResponse<object>
            {
                Success = true,
                Data = result
            });
        }

        return BadRequest(new CustomResponse<object>
        {
            Success = false,
            Data = null,
            Messages = _notifier.GetNotifications()
                .Select(n => new CustomMessage { Message = n.Message })
                .ToList()
        });
    }
    protected void NotifyError(string message)
    {
        _notifier.Handle(new Notification(message));
    }

    protected IActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
        {
            NotifyModelStateErrors(modelState);
        }

        return CustomResponse();
    }
}
