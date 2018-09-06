using System;

namespace Product.Api.Contract
{
    public class ViewProduct : CreateProduct
    {
        public DateTime LastUpdated { get; set; }
        public int Id { get; set; }
    }
}
