using System.Threading.Tasks;

namespace Product.Api.Handlers.Command
{
    public interface ICommandHander<in TCommand>
    {
        Task Execute(TCommand query);
    }
}
