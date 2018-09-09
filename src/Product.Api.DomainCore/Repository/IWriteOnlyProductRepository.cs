using System.Threading.Tasks;

namespace Product.Api.DomainCore.Repository
{
    public interface IWriteOnlyProductRepository
    {
        Task SaveProductAsync(Models.Product product);
        Task DeleteAsync(Models.Product product);
        Task UpdateAsync();
    }
}
