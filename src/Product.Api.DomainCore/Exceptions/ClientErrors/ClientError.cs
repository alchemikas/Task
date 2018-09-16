using System.Net;

namespace Product.Api.DomainCore.Exceptions.ClientErrors
{
    public class ClientError : ApiError
    {
        public ClientError(Fault fault, HttpStatusCode statusCode) : base(fault, statusCode)
        {
        }

        public ClientError(HttpStatusCode statusCode) : base(statusCode)
        {
        }
    }
}
