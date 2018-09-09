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

        private readonly IWriteOnlyProductRepository _writeOnlyProductRepository;
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;

        public UpdateProductHandler(IWriteOnlyProductRepository writeOnlyProductRepository,
            IReadOnlyProductRepository readOnlyProductRepository)
        {
            _writeOnlyProductRepository = writeOnlyProductRepository;
            _readOnlyProductRepository = readOnlyProductRepository;
        }

        protected override async Task HandleCommand(UpdateProductCommand command)
        {
            Models.Product product = await _readOnlyProductRepository.GetProductAsync(command.ProductId).ConfigureAwait(false);
            if (product == null) throw new NotFoundException(new List<Fault>{new Fault{Reason = "ResourceNotFound", Message = "Resource not found."}});

            bool wasCodeChanged = product.Code != command.Code;
            if (wasCodeChanged)
            {
                await ValidateProductCode(_faults, command.Code);
                if (_faults.Any()) throw new ValidationException(_faults);
            }

            product.LastUpdated = DateTime.Now;
            product.Name = command.Name;
            product.Code = command.Code;
            product.Price = command.Price;

            if (!string.IsNullOrEmpty(command.FileContent) && !string.IsNullOrEmpty(command.FileTitle))
            {
                if(product.Image == null) product.Image = new File();

                product.Image.Content = Convert.FromBase64String(command.FileContent);
                product.Image.ContentType = command.FileContentType;
                product.Image.Title = command.FileTitle;
            }

            await _writeOnlyProductRepository.UpdateAsync().ConfigureAwait(false);

        }

        protected override async Task Validate(UpdateProductCommand command, List<Fault> faults)
        {
            ProductValidator.ValidateFile(faults, command.FileContent);
            ProductValidator.ValidateProductName(faults, command.Name);
            ProductValidator.ValidateProductPrice(faults, command.Price);
            await Task.CompletedTask;
        }

        public async Task ValidateProductCode(List<Fault> faults, string code)
        {
            if (string.IsNullOrEmpty(code)) faults.Add(new Fault() {Reason = nameof(code), Message = ProductValidator.CODE_IS_REQUIRED });
            else
            {
                Models.Product product = await _readOnlyProductRepository.GetProductByCodeAsync(code).ConfigureAwait(false);
                if (product != null)
                {
                    faults.Add(new Fault() {Reason = nameof(code), Message = ProductValidator.CODE_MUST_BE_UNIQUE });
                }
            }
        }
    }
}
