using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Handlers.Command.BaseCommand;
using Product.Api.DomainCore.Repository;
using Product.Api.DomainCore.Services;

namespace Product.Api.DomainCore.Handlers.Command
{

    public class CreateProductHandler : BaseCommandHandler<CreateProductCommand>
    {
        private readonly IWriteOnlyProductRepository _writeOnlyProductRepository;
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;
        private readonly IMapper _mapper;

        public CreateProductHandler(IWriteOnlyProductRepository writeOnlyProductRepository,
            IReadOnlyProductRepository readOnlyProductRepository,
            IMapper mapper)
        {
            _writeOnlyProductRepository = writeOnlyProductRepository;
            _readOnlyProductRepository = readOnlyProductRepository;
            _mapper = mapper;
        }

        protected override async Task HandleCommand(CreateProductCommand command)
        {
            Models.Product domainModel = _mapper.Map<Models.Product>(command);
            domainModel.LastUpdated = DateTime.Now;
            await _writeOnlyProductRepository.SaveProductAsync(domainModel);

            command.ProductId = domainModel.Id;
        }

        protected override async Task Validate(CreateProductCommand command, List<Fault> faults)
        {
            ProductValidator.ValidateFile(faults, command.FileContent);
            ProductValidator.ValidateProductName(faults, command.Name);
            ProductValidator.ValidateProductPrice(faults, command.Price);
            await ValidateProductCode(faults, command.Code);
        }

        public async Task ValidateProductCode(List<Fault> faults, string code)
        {
            if (string.IsNullOrEmpty(code)) faults.Add(new Fault() { Reason = nameof(code), Message = ProductValidator.CODE_IS_REQUIRED });
            else
            {
                Models.Product product = await _readOnlyProductRepository.GetProductByCodeAsync(code).ConfigureAwait(false);
                if (product != null)
                {
                    faults.Add(new Fault() { Reason = nameof(code), Message = ProductValidator.CODE_MUST_BE_UNIQUE });
                }
            }
        }
    }
}
