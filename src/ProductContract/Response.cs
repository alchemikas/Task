using System.Collections.Generic;

namespace Product.Api.Contract
{
    public class Response
    {
        public Response()
        {
            Errors = new List<Error>();
            Warrnings = new List<Warrning>();
        }

        public List<Error> Errors { get; set; }
        public List<Warrning> Warrnings { get; set; }
    }
}
