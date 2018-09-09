namespace Product.Api.Contract
{
    public class CreateProduct : Response
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ImageFile Photo { get; set; }
        public decimal Price { get; set; }
    }
}
