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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductValidationService _productValidationService;

        public CreateProductHandler(IProductRepository productRepository,
            IMapper mapper,
            IProductValidationService productValidationService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productValidationService = productValidationService;
        }

        protected override async Task HandleCommand(CreateProductCommand command)
        {
            Models.Product domainModel = _mapper.Map<Models.Product>(command);
            domainModel.LastUpdated = DateTime.Now;
            await _productRepository.SaveProduct(domainModel);
        }

        protected override async Task Validate(CreateProductCommand command, List<Fault> faults)
        {
            _productValidationService.ValidateProductName(faults, command.Product.Name);
            _productValidationService.ValidateProductPrice(faults, command.Product.Price);
            await _productValidationService.ValidateProductCode(faults, command.Product.Code);
        }
    }
}
