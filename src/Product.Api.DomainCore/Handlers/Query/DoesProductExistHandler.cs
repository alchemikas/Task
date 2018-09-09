using System.Threading.Tasks;
using Product.Api.DomainCore.Queries;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Handlers.Query
{
    public class DoesProductExistHandler : IQueryHandler<GetProductQuery, bool>
    {
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;

        public DoesProductExistHandler(IReadOnlyProductRepository readOnlyProductRepository)
        {
            _readOnlyProductRepository = readOnlyProductRepository;
        }

        public async Task<bool> Handle(GetProductQuery query)
        {
            if (query.ProductId == default(int)) return false;
            return await _readOnlyProductRepository.DoesProductExistAsync(query.ProductId);
        }
    }
}
