using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Exceptions;
using Product.Api.DomainCore.Handlers.BaseHandler;
using Product.Api.DomainCore.Handlers.Validators;

namespace Product.Api.DomainCore.Handlers
{
    public abstract class BaseProductModificationHandler<T> : BaseCommandHandler<T> where T : ProductModificationCommand
    {
        protected const int ThumbnailImageHeight = 50;
        protected const int ThumbnailImageWidth = 50;

        protected abstract override Task HandleCommand(T command);
        protected abstract override Task Validate(T command, List<Fault> faults);


        protected void ValidateProductModification(T command, List<Fault> faults)
        {
            ImageFileValidator.ValidateFileContent(faults, command.FileContent);
            ImageFileValidator.ValidateImageExtension(faults, command.FileTitle);
            ImageFileValidator.ValidateImageFileTitle(faults, command.FileTitle, command.FileContent);

            ProductValidator.ValidateProductName(faults, command.Name);
            ProductValidator.ValidateProductPrice(faults, command.Price);
        }
    }
}
