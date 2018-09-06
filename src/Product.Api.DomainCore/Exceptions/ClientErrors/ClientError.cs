using System;
using System.Collections.Generic;

namespace Product.Api.DomainCore.Exceptions.ClientErrors
{
    public class ClientError : Exception
    {
        public List<Fault> Faults;

        public ClientError(List<Fault> faults)
        {
            Faults = faults;
        }
    }
}
