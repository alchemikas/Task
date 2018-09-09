namespace Product.Api.DomainCore.Commands
{
    public class CreateProductCommand
    {
        public string FileTitle { get; set; }
        public string FileContentType { get; set; }
        public string FileContent { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int ProductId { get; set; }
    }
}
