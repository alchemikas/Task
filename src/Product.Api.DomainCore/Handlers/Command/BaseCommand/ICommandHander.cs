using System.Threading.Tasks;

namespace Product.Api.DomainCore.Handlers.Command.BaseCommand
{
    public interface ICommandHander<in TCommand>
    {
        Task Execute(TCommand query);
    }
}
