using System;

namespace Product.Api.DomainCore.Services
{
    public class FileValidationService : IFileValidationService
    {
        public bool IsValidBase64String(string fileContent)
        {
            try
            {
                Convert.FromBase64String(fileContent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
