using System.Threading.Tasks;
using Product.Api.DomainCore.Repository;
using Product.Api.ReadModel.Queries;

namespace Product.Api.ReadModel.Handlers
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
