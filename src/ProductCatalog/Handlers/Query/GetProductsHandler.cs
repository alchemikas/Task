using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.Contract.Request;
using Product.Api.Contract.Response;
using Product.Api.DomainCore.Repository;

namespace Product.Api.Handlers.Query
{
    public class GetProductsHandler : IQueryHandler<GetProductsRequest, ProductsResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductsResponse> Handle(GetProductsRequest request)
        {
            List<Models.Product> products = await _productRepository.GetProducts(request.SearchTerm);
            List<Contract.Models.Product> productsResponse = _mapper.Map<List<Contract.Models.Product>>(products);
            return new ProductsResponse {Products = productsResponse};
        }
    }
}
