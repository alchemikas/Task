using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.Contract.Request;
using Product.Api.DomainCore;
using Product.Api.DomainCore.Repository;

namespace Product.Api.Handlers.Command
{

    public class CreateProductHandler : BaseCommandHandler<CreateProductRequest>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        private const string CODE_IS_REQUIRED = "Field Code is required.";
        private const string CODE_MUST_BE_UNIQUE = "Code should be unique.";
        private const string NAME_IS_REQUIRED = "Field Name is required.";
        private const string PRICE_SHOULD_BE_IN_RANGE = "Price should be in range 0 - 999.";

        private const int MIN_PRICE_VALUE = 0;
        private const int MAX_PRICE_VALUE = 999;

        public CreateProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        protected override async Task HandleCommand(CreateProductRequest command)
        {
            Models.Product domainModel = _mapper.Map<Models.Product>(command);
            await _productRepository.SaveProduct(domainModel).ConfigureAwait(false);
        }

        protected override async Task<List<ValidationError>> Validate(CreateProductRequest command)
        {
            var validationErros = new List<ValidationError>();
            ValidateProductName(validationErros, command.Product.Name);
            ValidateProductPrice(validationErros, command.Product.Price);
            await ValidateProductCode(validationErros, command.Product.Code);
            return validationErros;
        }


        private void ValidateProductName(List<ValidationError> validationErrors, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                validationErrors.Add(new ValidationError() { Field = nameof(name), ValidationMessage = NAME_IS_REQUIRED });
            }
        }

        private void ValidateProductPrice(List<ValidationError> validationErrors, double price)
        {
            bool isPriceInRange = price < MIN_PRICE_VALUE || price > MAX_PRICE_VALUE;

            if (!isPriceInRange)
            {
                validationErrors.Add(new ValidationError() { Field = nameof(price), ValidationMessage = PRICE_SHOULD_BE_IN_RANGE });
            }
        }

        private async Task ValidateProductCode(List<ValidationError> validationErrors, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                validationErrors.Add(new ValidationError() { Field = nameof(code), ValidationMessage = CODE_IS_REQUIRED });
            }
            else
            {
                Models.Product product = await _productRepository.GetProductByCode(code).ConfigureAwait(false);
                if (product == null)
                {
                    validationErrors.Add(new ValidationError() { Field = nameof(code), ValidationMessage = CODE_IS_REQUIRED });
                }
            }
        }
    }
}
