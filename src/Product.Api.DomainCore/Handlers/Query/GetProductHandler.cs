using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Querys;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Handlers.Query
{
    public class GetProductHandler : IQueryHandler<GetProductQuery, GetProductDetailsResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductHandler(IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetProductDetailsResponse> Handle(GetProductQuery query)
        {
            if (query.ProductId != default(int))
            {
                Models.Product product = await _productRepository.GetProduct(query.ProductId);
                if (product != null)
                {
                    ViewProduct result = _mapper.Map<ViewProduct>(product);
                    return new GetProductDetailsResponse { Product = result };
                }
                throw new NotFoundException(new List<Fault>()
                {
                    new Fault {Reason = "NotFoundResource", Message = $"Product with id:{query.ProductId} not found."}
                });
            }

            if (!string.IsNullOrEmpty(query.Code))
            {
                Models.Product product = await _productRepository.GetProductByCode(query.Code);
                if (product != null)
                {
                    ViewProduct result = _mapper.Map<ViewProduct>(product);
                    return new GetProductDetailsResponse { Product = result };
                }
                throw new NotFoundException(new List<Fault>()
                {
                    new Fault {Reason = "NotFoundResource", Message = $"Product with code:{query.ProductId} not found."}
                });
            }

            throw new ValidationException(new List<Fault>()
            {
                new Fault {Reason = "NotValid", Message = $"Request is not valid."}
            });
        }
    }
}
