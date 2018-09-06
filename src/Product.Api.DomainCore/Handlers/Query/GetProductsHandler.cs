using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore.Querys;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Handlers.Query
{
    public class GetProductsHandler : IQueryHandler<GetProductsQuery, GetProductsResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsHandler(IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetProductsResponse> Handle(GetProductsQuery query)
        {
            List<Models.Product> products = await _productRepository.GetProducts(query.SearchTerm);
            var  productsResponse = _mapper.Map<List<Api.Contract.ViewProduct>>(products);
            return new GetProductsResponse() {Products = productsResponse};
        }
    }
}
