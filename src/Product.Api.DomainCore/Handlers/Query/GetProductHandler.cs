using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Queries;
using Product.Api.DomainCore.Repository;

namespace Product.Api.DomainCore.Handlers.Query
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
                Models.Product product = await _readOnlyProductRepository.GetProductAsync(query.ProductId);
                if (product != null)
                {
                    ViewProduct result = _mapper.Map<ViewProduct>(product);
                    return new ProductDetailsResponse { Product = result };
                }
                throw new NotFoundException(new List<Fault>()
                {
                    new Fault {Reason = "NotFoundResource", Message = $"Product with id:{query.ProductId} not found."}
                });
            }

            if (!string.IsNullOrEmpty(query.Code))
            {
                Models.Product product = await _readOnlyProductRepository.GetProductByCodeAsync(query.Code);
                if (product != null)
                {
                    ViewProduct result = _mapper.Map<ViewProduct>(product);
                    return new ProductDetailsResponse { Product = result };
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
