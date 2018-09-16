namespace Product.Api.DomainCore.Commands
{
    public abstract class ProductModificationCommand
    {
        public string FileTitle { get; set; }
        public string FileContentType { get; set; }
        public string FileContent { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}