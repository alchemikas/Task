using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Product.Api.DomainCore.Handlers.BaseHandler;

namespace Product.Api.LocalInfrastructure.Dispachers
{
    public interface ICommandDispacher
    {
        Task Execute<TCommand>(TCommand cmd);
    }

    public class CommandDispacher : ICommandDispacher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispacher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute<TCommand>(TCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentException(nameof(cmd));
            }
            var handler = _serviceProvider.GetService<ICommandHander<TCommand>>();

            if (handler == null)
            {
                throw new ArgumentException(nameof(cmd));
            }

            await handler.Execute(cmd).ConfigureAwait(false);
        }
    }
}
