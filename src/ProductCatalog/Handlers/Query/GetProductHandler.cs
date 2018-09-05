using System;
using System.Threading.Tasks;
using Product.Api.Contract.Models;
using Product.Api.Contract.Request;
using Product.Api.Contract.Response;
using Product.Api.DomainCore.Repository;

namespace Product.Api.Handlers.Query
{
    public class GetProductHandler : IQueryHandler<GetProductRequest, ProductResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> Handle(GetProductRequest request)
        {
            if (request.ProductId != default(int))
            {
                var product = await _productRepository.GetProduct(request.ProductId);
                if (product != null)
                {
                    return new ProductResponse()
                    {
                        Product = new Contract.Models.Product()
                        {
                            Code = product.Code,
                            Price = product.Price,
                            Id = product.Id,
                            Name = product.Name,
                            Photo = new ImageFile()
                            {
                                ContentType = product.Image.ContentType,
                                Title = product.Image.Title,
                                Content = Convert.ToBase64String(product.Image.Content)
                            }
                        }
                    };
                }
                throw new Exception("");
            }
            throw new Exception("");
        }
    }
}
