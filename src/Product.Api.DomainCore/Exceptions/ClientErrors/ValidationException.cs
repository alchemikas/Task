using System.Collections.Generic;
using System.Net;

namespace Product.Api.DomainCore.Exceptions.ClientErrors
{
    public class ValidationException : ClientError
    {
        public ValidationException(Fault fault) : base(fault, HttpStatusCode.BadRequest)
        {
        }

        public ValidationException() : base(HttpStatusCode.BadRequest)
        {
        }
    }
}
