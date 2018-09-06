using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Services
{
    // TODO could be splited in separate validation services like for name, price and validation for code
    // so who just use name, price validation should not have dependency on repo
    public class ProductValidationService : IProductValidationService
    {
        private readonly IProductRepository _productRepository;

        private const string CODE_IS_REQUIRED = "Field Code is required.";
        private const string CODE_MUST_BE_UNIQUE = "Code should be unique.";
        private const string NAME_IS_REQUIRED = "Field Name is required.";
        private const string PRICE_SHOULD_BE_IN_RANGE = "Price should be in range 0 - 999.";

        private const int MIN_PRICE_VALUE = 0;
        private const int MAX_PRICE_VALUE = 999;

        public ProductValidationService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void ValidateProductName(List<Fault> faults, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                faults.Add(new Fault() { Reason = nameof(name), Message = NAME_IS_REQUIRED });
            }
        }

        public void ValidateProductPrice(List<Fault> faults, decimal price)
        {
            bool isPriceInRange = price >= MIN_PRICE_VALUE || price <= MAX_PRICE_VALUE;

            if (!isPriceInRange)
            {
                faults.Add(new Fault() { Reason = nameof(price), Message = PRICE_SHOULD_BE_IN_RANGE });
            }
        }

        public async Task ValidateProductCode(List<Fault> faults, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                faults.Add(new Fault() { Reason = nameof(code), Message = CODE_IS_REQUIRED });
            }
            else
            {
                Models.Product product = await _productRepository.GetProductByCode(code).ConfigureAwait(false);
                if (product != null)
                {
                    faults.Add(new Fault() { Reason = nameof(code), Message = CODE_MUST_BE_UNIQUE });
                }
            }
        }
    }
}
