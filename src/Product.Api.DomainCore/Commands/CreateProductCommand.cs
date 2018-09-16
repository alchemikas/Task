namespace Product.Api.DomainCore.Commands
{
    public class CreateProductCommand : ProductModificationCommand
    {
        // output id
        public int ProductId { get; set; }
    }
}
