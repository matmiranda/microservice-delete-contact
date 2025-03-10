using System.Net;

namespace DeletarContatos.Infrastructure.Exceptions
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public CustomException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
