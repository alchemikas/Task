namespace Product.Api.DomainCore.Commands
{
    public class UpdateProductCommand : ProductModificationCommand
    {
        public int ProductId { get; set; }
    }
}
