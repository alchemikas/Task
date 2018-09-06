using System.Threading.Tasks;
using Product.Api.DomainCore.Querys;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Handlers.Query
{
    public class DoesProductExistHandler : IQueryHandler<GetProductQuery, bool>
    {
        private readonly IProductRepository _productRepository;

        public DoesProductExistHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(GetProductQuery query)
        {
            if (query.ProductId == default(int)) return false;
            return await _productRepository.DoesProductExist(query.ProductId);
        }
    }
}
