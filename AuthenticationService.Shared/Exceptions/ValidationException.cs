
using AuthenticationService.Shared.Resources;

namespace AuthenticationService.Shared.Exceptions
{
    /// <summary>
    /// Represents an exception that is intended to be returned in an API response with a status code of <see cref="HttpStatusCode.BadRequest"/>.
    /// This exception is typically used to indicate that one or more validation errors occurred during the processing of a request.
    /// </summary>
    public class ValidationException : ApplicationException
    {
        public ValidationException() : base(GeneralMessages.ValidationExceptionMessage)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(Dictionary<string, string[]> errors) : this()
        {
            Errors = errors;              
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
