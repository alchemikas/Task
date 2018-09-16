using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;

namespace Product.Api.DomainCore.Exceptions
{
    public abstract class ApiError : Exception
    {
        private readonly List<Fault> _faults = new List<Fault>();
        public HttpStatusCode StatusCode { get; }


        protected ApiError(Fault fault, HttpStatusCode statusCode)
        {
            _faults.Add(fault);
            StatusCode = statusCode;
        }

        protected ApiError(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }


        public ReadOnlyCollection<Fault> GetErrors()
        {
            return _faults.AsReadOnly();
        }

        public void AddError(Fault fault)
        {
            _faults.Add(fault);
        }

        public void AddErrors(List<Fault> faults)
        {
            _faults.AddRange(faults);
        }
    }
}
