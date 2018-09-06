using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Handlers.Command.BaseCommand;
using Product.Api.DomainCore.Repository;
using Product.Api.DomainCore.Services;

namespace Product.Api.DomainCore.Handlers.Command
{
    public class UpdateProductHandler : BaseCommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductValidationService _productValidationService;

        public UpdateProductHandler(IProductRepository productRepository,
            IProductValidationService productValidationService)
        {
            _productRepository = productRepository;
            _productValidationService = productValidationService;
        }

        protected override async Task HandleCommand(UpdateProductCommand command)
        {
            Models.Product product = await _productRepository.GetProduct(command.Product.Id).ConfigureAwait(false);
            if (product == null) throw new NotFoundException(new List<Fault>{new Fault{Reason = "ResourceNotFound", Message = "Resource not found."}});

            product.LastUpdated = DateTime.Now;
            product.Code = command.Product.Code;
            product.Name = command.Product.Name;
            product.Price = command.Product.Price;

            if (command.Product.Image != null)
            {
                product.Image.Content = command.Product.Image.Content;
                product.Image.ContentType = command.Product.Image.ContentType;
                product.Image.Title = command.Product.Image.Title;
            }

            await _productRepository.Update().ConfigureAwait(false);

        }

        protected override async Task Validate(UpdateProductCommand command, List<Fault> faults)
        {
            _productValidationService.ValidateProductName(faults, command.Product.Name);
            _productValidationService.ValidateProductPrice(faults, command.Product.Price);
            await _productValidationService.ValidateProductCode(faults, command.Product.Code);
        }
    }
}
