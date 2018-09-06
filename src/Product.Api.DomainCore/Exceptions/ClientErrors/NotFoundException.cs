using System.Collections.Generic;

namespace Product.Api.DomainCore.Exceptions.ClientErrors
{
    public class NotFoundException : ClientError
    {
        public NotFoundException(List<Fault> faults) : base(faults)
        {
        }

    }
}
