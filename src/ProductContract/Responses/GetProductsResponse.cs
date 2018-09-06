using System.Collections.Generic;

namespace Product.Api.Contract.Responses
{
    public class GetProductsResponse : Response
    {
        public List<ViewProduct> Products { get; set; }
    }
}
