using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.Contract.Request;
using Product.Api.DomainCore;
using Product.Api.DomainCore.Repository;

namespace Product.Api.Handlers.Command
{
    public class UpdateProductHandler : BaseCommandHandler<UpdateProductRequest>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        protected override async Task HandleCommand(UpdateProductRequest command)
        {
            Models.Product product = await _productRepository.GetProduct(command.Product.Id).ConfigureAwait(false);
            if (product == null)
            {
                throw new Exception("Not Found");
            }

            await _productRepository.Delete(product).ConfigureAwait(false);
        }

        protected override async Task<List<ValidationError>> Validate(UpdateProductRequest command)
        {
            List<ValidationError> validationErrors = new List<ValidationError>();
            return validationErrors;
        }
    }
}
