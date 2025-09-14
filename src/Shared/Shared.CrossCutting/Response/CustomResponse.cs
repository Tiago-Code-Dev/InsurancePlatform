namespace Shared.CrossCutting.Response;

public class CustomResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<CustomMessage> Messages { get; set; } = new();

    public static CustomResponse<T> Ok(T data)
    {
        return new CustomResponse<T>
        {
            Success = true,
            Data = data
        };
    }

    public static CustomResponse<T> Created(T data)
    {
        return Ok(data);
    }

    public static CustomResponse<T> Fail(string message)
    {
        return new CustomResponse<T>
        {
            Success = false,
            Messages = new List<CustomMessage> { new() { Message = message } }
        };
    }

    public static CustomResponse<T> Fail(List<CustomMessage> messages)
    {
        return new CustomResponse<T>
        {
            Success = false,
            Messages = messages
        };
    }

    public static CustomResponse<T> ValidationFail(Dictionary<string, string> errors)
    {
        var messages = errors.Select(e => new CustomMessage
        {
            Field = e.Key,
            Message = e.Value
        }).ToList();

        return Fail(messages);
    }

    public static CustomResponse<T> InternalServerError()
    {
        return Fail("An internal error has occurred. Please try again later.");
    }
}

public class CustomMessage
{
    public string? Field { get; set; }
    public string Message { get; set; } = string.Empty;
}
