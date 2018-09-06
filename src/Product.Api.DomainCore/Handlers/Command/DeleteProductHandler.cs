using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Handlers.Command.BaseCommand;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Handlers.Command
{
    public class DeleteProductHandler : BaseCommandHandler<DeleteProductCommand>
    {
        private const string PRODUCT_ID_IS_NOT_PROVIDED = "Product id is not provided.";
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        protected override async Task HandleCommand(DeleteProductCommand command)
        {
            Models.Product product = await _productRepository.GetProduct(command.ProductId).ConfigureAwait(false);
            if (product == null)
            {
                throw new Exception("Not Found");
            }

            await _productRepository.Delete(product).ConfigureAwait(false);
        }

        protected override Task Validate(DeleteProductCommand command, List<Fault> faults)
        {
            ValidateProductId(faults, command.ProductId);
            return Task.CompletedTask;
        }

        private void ValidateProductId(List<Fault> faults, int id)
        {
            if (id == default(int))
            {
                faults.Add(new Fault(){ Reason = nameof(id), Message = PRODUCT_ID_IS_NOT_PROVIDED });
            }
        }


      
    }
}
