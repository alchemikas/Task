using System.Net;

namespace Product.Api.DomainCore.Exceptions.ClientErrors
{
    public class NotFoundException : ClientError
    {
        public NotFoundException(Fault fault) : base(fault, HttpStatusCode.NotFound)
        {
        }

    }
}
