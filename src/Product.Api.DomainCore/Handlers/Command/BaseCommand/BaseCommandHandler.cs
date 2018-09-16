using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Api.DomainCore.Exceptions;
using Product.Api.DomainCore.Exceptions.ClientErrors;

namespace Product.Api.DomainCore.Handlers.Command.BaseCommand
{
    public abstract class BaseCommandHandler<TCommand> : ICommandHander<TCommand>
    {
        protected List<Fault> Faults;
        protected BaseCommandHandler()
        {
            Faults = new List<Fault>();
        }

        public async Task Execute(TCommand command)
        {
            await Validate(command, Faults);
            if (Faults.Any())
            {
                var exception = new ValidationException();
                exception.AddErrors(Faults);
                throw exception;
            }

            await HandleCommand(command);
        }

        protected abstract Task HandleCommand(TCommand command);

        protected abstract Task Validate(TCommand command, List<Fault> faults);
        
    }
}
