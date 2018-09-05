namespace Product.Api.DomainCore
{
    public class ValidationError
    {
        public string Field { get; set; }
        public string ValidationMessage { get; set; }
    }
}
