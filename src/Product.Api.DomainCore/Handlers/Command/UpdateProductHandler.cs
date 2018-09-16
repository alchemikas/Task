using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Exceptions;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Handlers.Command.BaseCommand;
using Product.Api.DomainCore.Models;
using Product.Api.DomainCore.Repository;
using Product.Api.DomainCore.Services;

namespace Product.Api.DomainCore.Handlers.Command
{
    public class UpdateProductHandler : BaseCommandHandler<UpdateProductCommand>
    {
        private const int ThumbnailImageHeight = 50;
        private const int ThumbnailImageWidth = 50;
        private const string PRODUCT_NOT_FOUND = "Resource not found.";

        private readonly IWriteOnlyProductRepository _writeOnlyProductRepository;
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;
        private readonly IImageFileResizeService _imageFileResizeService;

        public UpdateProductHandler(IWriteOnlyProductRepository writeOnlyProductRepository,
            IReadOnlyProductRepository readOnlyProductRepository,
            IImageFileResizeService imageFileResizeService)
        {
            _writeOnlyProductRepository = writeOnlyProductRepository;
            _readOnlyProductRepository = readOnlyProductRepository;
            _imageFileResizeService = imageFileResizeService;
        }

        protected override async Task HandleCommand(UpdateProductCommand command)
        {
            Models.Product product = await _readOnlyProductRepository.GetProductAsync(command.ProductId).ConfigureAwait(false);
            if (product == null) throw new NotFoundException(new Fault{Reason = "ResourceNotFound", Message = PRODUCT_NOT_FOUND });

            bool wasCodeChanged = product.Code != command.Code;
            if (wasCodeChanged)
            {
                var productByCode = await _readOnlyProductRepository.GetProductByCodeAsync(command.Code).ConfigureAwait(false);
                ProductValidator.ValidateProductCode(Faults, productByCode, command.Code);
                if (Faults.Any())
                {
                    var exception = new ValidationException();
                    exception.AddErrors(Faults);
                    throw exception;
                }
            }

            product.LastUpdated = DateTime.Now;
            product.Name = command.Name;
            product.Code = command.Code;
            product.Price = command.Price;

            if (!string.IsNullOrEmpty(command.FileContent) && !string.IsNullOrEmpty(command.FileTitle))
            {
                byte[] imageBytes = Convert.FromBase64String(command.FileContent);

                HandleProductImage(command, product, imageBytes);
                HandleProductImageThumbnail(command, product, imageBytes);
            }

            await _writeOnlyProductRepository.UpdateAsync().ConfigureAwait(false);

        }

        private void HandleProductImageThumbnail(UpdateProductCommand command, Models.Product product, byte[] imageBytes)
        {
            if (product.ImageThumbnail == null) product.ImageThumbnail = new ImageThumbnail();
            product.ImageThumbnail.Content = _imageFileResizeService.ResizeImage(imageBytes, ThumbnailImageHeight, ThumbnailImageWidth);
            product.ImageThumbnail.ContentType = command.FileContentType;
            product.ImageThumbnail.Title = command.FileTitle;
        }

        private static void HandleProductImage(UpdateProductCommand command, Models.Product product, byte[] imageBytes)
        {
            if (product.Image == null) product.Image = new Image();
            product.Image.Content = imageBytes;
            product.Image.ContentType = command.FileContentType;
            product.Image.Title = command.FileTitle;
        }

        protected override async Task Validate(UpdateProductCommand command, List<Fault> faults)
        {
            ImageFileValidator.ValidateFileContent(faults, command.FileContent);
            ImageFileValidator.ValidateImageExtension(faults, command.FileTitle);
            ImageFileValidator.ValidateImageFileTitle(faults, command.FileTitle, command.FileContent);

            ProductValidator.ValidateProductName(faults, command.Name);
            ProductValidator.ValidateProductPrice(faults, command.Price);
            await Task.CompletedTask;
        }
    }
}
