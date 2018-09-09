using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Handlers.Command.BaseCommand;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Handlers.Command
{
    public class DeleteProductHandler : BaseCommandHandler<DeleteProductCommand>
    {
        private const string PRODUCT_ID_IS_NOT_PROVIDED = "Product id is not provided.";
        private readonly IWriteOnlyProductRepository _writeOnlyProductRepository;
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;

        public DeleteProductHandler(IWriteOnlyProductRepository writeOnlyProductRepository,
            IReadOnlyProductRepository readOnlyProductRepository)
        {
            _writeOnlyProductRepository = writeOnlyProductRepository;
            _readOnlyProductRepository = readOnlyProductRepository;
        }

        protected override async Task HandleCommand(DeleteProductCommand command)
        {
            Models.Product product = await _readOnlyProductRepository.GetProductAsync(command.ProductId).ConfigureAwait(false);
            if (product == null)
            {
                throw new NotFoundException(new List<Fault>(){new Fault(){Reason = "NotFound", Message = "Resource not found."}});
            }

            await _writeOnlyProductRepository.DeleteAsync(product).ConfigureAwait(false);
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
