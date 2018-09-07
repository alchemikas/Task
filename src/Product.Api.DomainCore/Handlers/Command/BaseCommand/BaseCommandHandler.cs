using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Api.DomainCore.Exceptions.ClientErrors;

namespace Product.Api.DomainCore.Handlers.Command.BaseCommand
{

    // Todo could be added restriction for commands that all commands inherit from single class
    // Todo wrap Execute into transaction if more complex operration will be apllyed
    public abstract class BaseCommandHandler<TCommand> : ICommandHander<TCommand>
    {
        protected List<Fault> _faults;
        protected BaseCommandHandler()
        {
            _faults = new List<Fault>();
        }

        public async Task Execute(TCommand command)
        {
            await Validate(command, _faults);
            if (_faults.Any())
            {
                throw new ValidationException(_faults); 
            }

            await HandleCommand(command);
        }

        protected abstract Task HandleCommand(TCommand command);

        protected abstract Task Validate(TCommand command, List<Fault> _faults);
        
    }
}
