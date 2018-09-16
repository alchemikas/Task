using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore.Repository;
using Product.Api.ReadModel.Queries;

namespace Product.Api.ReadModel.Handlers
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
            List<DomainCore.Models.Product> products = await _readOnlyProductRepository.GetProductsAsync(query.SearchTerm);

            var response =  new GetProductsResponse(){Products = new List<ViewProduct>()};
            foreach (var product in products)
            {
                var responseProduct = _mapper.Map<Contract.ViewProduct>(product);
                if (product.ImageThumbnail != null)
                {
                    responseProduct.Photo = _mapper.Map<ImageFile>(product.ImageThumbnail);
                }
                response.Products.Add(responseProduct);
            }

            return response;
        }
    }
}
