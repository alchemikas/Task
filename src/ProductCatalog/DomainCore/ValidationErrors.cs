using System.Collections.Generic;

namespace Product.Api.DomainCore
{
    public class ValidationErrors
    {
        public List<ValidationError> Errors { get; set; }
    }
}
