using System.Collections.Generic;

namespace Product.Api.DomainCore.Exceptions.ClientErrors
{
    public class ValidationException : ClientError
    {
        public ValidationException(List<Fault> faults) : base(faults)
        {
        }
    }
}
