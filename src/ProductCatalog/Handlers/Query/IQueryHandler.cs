using System.Threading.Tasks;

namespace Product.Api.Handlers
{
    public interface IQueryHandler<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
