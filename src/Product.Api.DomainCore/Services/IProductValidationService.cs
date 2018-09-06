using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Api.DomainCore.Services
{
    public interface IProductValidationService
    {
        void ValidateProductName(List<Fault> faults, string productName);
        void ValidateProductPrice(List<Fault> faults, decimal productPrice);
        Task ValidateProductCode(List<Fault> faults, string productCode);
    }
}
