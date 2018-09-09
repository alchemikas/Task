using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Api.DomainCore.Repository
{
    public interface IReadOnlyProductRepository
    {
        Task<Models.Product> GetProductAsync(int productId);
        Task<Models.Product> GetProductByCodeAsync(string code);
        Task<List<Models.Product>> GetProductsAsync(string searchTerm = null);
        Task<bool> DoesProductExistAsync(int id);
    }
}