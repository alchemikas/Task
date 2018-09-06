using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Api.DomainCore.Repository
{
    public interface IRepository
    {
    }

    public interface IProductRepository : IRepository
    {
        Task<Models.Product> GetProduct(int productId);
        Task<List<Models.Product>> GetProducts(string searchTerm = null);
        Task SaveProduct(Models.Product product);
        Task<Models.Product> GetProductByCode(string code);
        Task Delete(Models.Product product);
        Task<bool> DoesProductExist(int id);
        Task Update();
    }
}
