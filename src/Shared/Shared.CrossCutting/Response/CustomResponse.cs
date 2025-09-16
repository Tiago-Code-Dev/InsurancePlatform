using System.Text.Json.Serialization;

namespace Shared.CrossCutting.Response;

public interface ICustomResponse
{
    int StatusCode { get; }
}

public class CustomResponse<T> : ICustomResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("messages")]
    public List<MessageResponse> Messages { get; set; } = new();

    [JsonIgnore]
    public int StatusCode { get; private set; }

    private CustomResponse() { }

    public static CustomResponse<T> Ok(T data) => new()
    {
        Success = true,
        Data = data,
        StatusCode = 200
    };

    public static CustomResponse<T> Created(T data) => new()
    {
        Success = true,
        Data = data,
        StatusCode = 201
    };

    public static CustomResponse<T> Fail(string message, string? field = null) => new()
    {
        Success = false,
        Data = default,
        Messages = new() { new MessageResponse(field, message) },
        StatusCode = 400
    };

    public static CustomResponse<T> Fail(IEnumerable<MessageResponse> messages) => new()
    {
        Success = false,
        Data = default,
        Messages = messages.ToList(),
        StatusCode = 400
    };

    public static CustomResponse<T> InternalServerError(string? message = null) => new()
    {
        Success = false,
        Data = default,
        Messages = new()
        {
            new MessageResponse(null, message ?? "An internal error has occurred. Please try again later.")
        },
        StatusCode = 500
    };
}

public class MessageResponse
{
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    public MessageResponse(string? field, string message)
    {
        Field = field;
        Message = message;
    }
}
