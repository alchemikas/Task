using System.Threading.Tasks;

namespace Product.Api.ReadModel.Handlers
{
    public interface IQueryHandler<in TQuery, TResponse>
    {
        Task<TResponse> Handle(TQuery query);
    }
}
