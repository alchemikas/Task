using System.Threading.Tasks;

namespace Product.Api.DomainCore.Handlers.BaseHandler
{
    public interface ICommandHander<in TCommand>
    {
        Task Execute(TCommand query);
    }
}
