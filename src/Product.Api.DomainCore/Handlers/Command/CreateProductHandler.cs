using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Exceptions;
using Product.Api.DomainCore.Handlers.Command.BaseCommand;
using Product.Api.DomainCore.Models;
using Product.Api.DomainCore.Repository;
using Product.Api.DomainCore.Services;

namespace Product.Api.DomainCore.Handlers.Command
{

    public class CreateProductHandler : BaseCommandHandler<CreateProductCommand>
    {
        private const int ThumbnailImageHeight = 50;
        private const int ThumbnailImageWidth = 50;

        private readonly IWriteOnlyProductRepository _writeOnlyProductRepository;
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;
        private readonly IImageFileResizeService _imageFileResizeService;
        private readonly IMapper _mapper;

        public CreateProductHandler(IWriteOnlyProductRepository writeOnlyProductRepository,
            IReadOnlyProductRepository readOnlyProductRepository,
            IImageFileResizeService imageFileResizeService,
            IMapper mapper)
        {
            _writeOnlyProductRepository = writeOnlyProductRepository;
            _readOnlyProductRepository = readOnlyProductRepository;
            _imageFileResizeService = imageFileResizeService;
            _mapper = mapper;
        }

        protected override async Task HandleCommand(CreateProductCommand command)
        {
            Models.Product domainModel = _mapper.Map<Models.Product>(command);
            domainModel.LastUpdated = DateTime.Now;

            if (domainModel.Image != null)
            {
                domainModel.ImageThumbnail = new ImageThumbnail
                {
                    Content = _imageFileResizeService.ResizeImage(domainModel.Image.Content, ThumbnailImageHeight, ThumbnailImageWidth),
                    Title = domainModel.Image.Title,
                    ContentType = domainModel.Image.ContentType
                };
            }

            await _writeOnlyProductRepository.SaveProductAsync(domainModel);

            command.ProductId = domainModel.Id;
        }

        protected override async Task Validate(CreateProductCommand command, List<Fault> faults)
        {
            ImageFileValidator.ValidateImageFileTitle(faults, command.FileTitle, command.FileContent);
            ImageFileValidator.ValidateImageExtension(faults, command.FileTitle);
            ImageFileValidator.ValidateFileContent(faults, command.FileContent);

            ProductValidator.ValidateProductName(faults, command.Name);
            ProductValidator.ValidateProductPrice(faults, command.Price);

            var product = await _readOnlyProductRepository.GetProductByCodeAsync(command.Code).ConfigureAwait(false);
            ProductValidator.ValidateProductCode(faults, product, command.Code);
        }
    }
}
