using System.Threading.Tasks;

namespace Product.Api.DomainCore.Handlers.Query
{
    public interface IQueryHandler<in TQuery, TResponse>
    {
        Task<TResponse> Handle(TQuery query);
    }
}
