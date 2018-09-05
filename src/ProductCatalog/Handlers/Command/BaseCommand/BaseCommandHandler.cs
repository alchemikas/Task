using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Api.DomainCore;

namespace Product.Api.Handlers.Command
{
    public abstract class BaseCommandHandler<TCommand> : ICommandHander<TCommand>
    {
        protected BaseCommandHandler()
        {
        }

        public async Task Execute(TCommand command)
        {
            List<ValidationError> validationErrors = await Validate(command);
            if (!validationErrors.Any())
            {
                 await HandleCommand(command);
            }
            throw new Exception();
        }

        protected abstract Task HandleCommand(TCommand command);

        protected abstract Task<List<ValidationError>> Validate(TCommand command);
        
    }
}
