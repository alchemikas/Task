namespace Product.Api.DomainCore.Services
{
    public interface IFileValidationService
    {
        bool IsValidBase64String(string fileContent);
    }
}
