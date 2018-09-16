using System.Net;

namespace Product.Api.DomainCore.Exceptions
{
    public class ServerError : ApiError
    {
        public ServerError(Fault fault) : base(fault, HttpStatusCode.InternalServerError)
        {
        }
    }
}
