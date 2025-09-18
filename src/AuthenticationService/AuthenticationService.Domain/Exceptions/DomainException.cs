using Shared.CrossCutting.Response;

namespace AuthenticationService.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string? Field { get; }


        public DomainException(string message, string? field = null)
        : base(message)
        {
            Field = field;
        }

        public MessageResponse ToMessageResponse() =>
            new MessageResponse(Field, Message);  
    }
}
