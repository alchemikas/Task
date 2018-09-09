using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore.Queries;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Handlers.Query
{
    public class GetProductsHandler : IQueryHandler<GetProductsQuery, GetProductsResponse>
    {
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;
        private readonly IMapper _mapper;

        public GetProductsHandler(IReadOnlyProductRepository readOnlyProductRepository,
            IMapper mapper)
        {
            _readOnlyProductRepository = readOnlyProductRepository;
            _mapper = mapper;
        }

        public async Task<GetProductsResponse> Handle(GetProductsQuery query)
        {
            List<Models.Product> products = await _readOnlyProductRepository.GetProductsAsync(query.SearchTerm);
            var  productsResponse = _mapper.Map<List<Api.Contract.ViewProduct>>(products);
            return new GetProductsResponse() {Products = productsResponse};
        }
    }
}
