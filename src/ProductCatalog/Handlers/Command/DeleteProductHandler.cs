using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.Contract.Request;
using Product.Api.DomainCore;
using Product.Api.DomainCore.Repository;

namespace Product.Api.Handlers.Command
{
    public class DeleteProductHandler : BaseCommandHandler<DeleteProductRequest>
    {
        private const string PRODUCT_ID_IS_NOT_PROVIDED = "Product id is not provided.";
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        protected override async Task HandleCommand(DeleteProductRequest command)
        {
            Models.Product product = await _productRepository.GetProduct(command.ProductId).ConfigureAwait(false);
            if (product == null)
            {
                throw new Exception("Not Found");
            }

            await _productRepository.Delete(product).ConfigureAwait(false);
        }

        protected override async Task<List<ValidationError>> Validate(DeleteProductRequest command)
        {
            List<ValidationError> validationErrors = new List<ValidationError>();
            ValidateProductId(validationErrors, command.ProductId);
            return validationErrors;
        }

        private void ValidateProductId(List<ValidationError> validationErrors, int id)
        {
            if (id == default(int))
            {
                validationErrors.Add(new ValidationError(){Field = nameof(id), ValidationMessage = PRODUCT_ID_IS_NOT_PROVIDED });
            }
        }


      
    }
}
