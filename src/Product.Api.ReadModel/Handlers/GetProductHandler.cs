using System.Threading.Tasks;
using AutoMapper;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore.Exceptions;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Repository;
using Product.Api.ReadModel.Queries;

namespace Product.Api.ReadModel.Handlers
{
    public class GetProductHandler : IQueryHandler<GetProductQuery, ProductDetailsResponse>
    {
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;
        private readonly IMapper _mapper;

        public GetProductHandler(IReadOnlyProductRepository readOnlyProductRepository,
            IMapper mapper)
        {
            _readOnlyProductRepository = readOnlyProductRepository;
            _mapper = mapper;
        }

        public async Task<ProductDetailsResponse> Handle(GetProductQuery query)
        {
            if (query.ProductId != default(int))
            {
                DomainCore.Models.Product product = await _readOnlyProductRepository.GetProductAsync(query.ProductId);
                if (product != null)
                {
                    return CreateProductResponse(product);
                }

                throw new NotFoundException(new Fault { Reason = "NotFoundResource", Message = $"Product with id:{query.ProductId} not found." });
            }

            if (!string.IsNullOrEmpty(query.Code))
            {
                DomainCore.Models.Product product = await _readOnlyProductRepository.GetProductByCodeAsync(query.Code);
                if (product != null)
                {
                    return CreateProductResponse(product);
                }

                throw new NotFoundException(new Fault { Reason = "NotFoundResource", Message = $"Product with code:{query.ProductId} not found." });
            }

            throw new ValidationException(new Fault { Reason = "NotValid", Message = $"Request is not valid." });
        }

        private ProductDetailsResponse CreateProductResponse(DomainCore.Models.Product product)
        {
            ViewProduct result = _mapper.Map<ViewProduct>(product);
            if (product.Image != null)
            {
                result.Photo = _mapper.Map<ImageFile>(product.Image);
            }
            return new ProductDetailsResponse {Product = result};
        }
    }
}
