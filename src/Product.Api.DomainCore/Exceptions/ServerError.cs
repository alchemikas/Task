using System;
using System.Collections.Generic;

namespace Product.Api.DomainCore.Exceptions
{
    public class ServerError : Exception
    {
        public List<Fault> Faults;

        public ServerError(List<Fault> faults)
        {
            Faults = faults;
        }
    }
}
