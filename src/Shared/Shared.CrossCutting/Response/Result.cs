

namespace Shared.CrossCutting.Response;

public class Result
{
    public string Message { get; set; } = string.Empty;

    public static Result Ok(string message = "Operation completed.") => new Result { Message = message };

    public static Result Success(string message = "Operation successfully completed.")
    {
        return new Result { Message = message };
    }
}
