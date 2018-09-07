using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Handlers.Command.BaseCommand;
using Product.Api.DomainCore.Models;
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

            if (product.Code != command.Product.Code)
            {
                await _productValidationService.ValidateProductCode(_faults, command.Product.Code);
                if (_faults.Any()) throw new ValidationException(_faults);
            }

            product.LastUpdated = DateTime.Now;
            product.Name = command.Product.Name;
            product.Code = command.Product.Code;
            product.Price = command.Product.Price;

            if (command.Product.Image != null)
            {
                if(product.Image == null) product.Image = new File();

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
            await Task.CompletedTask;
        }
    }
}
