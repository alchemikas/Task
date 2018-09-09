using System.Threading.Tasks;
using Product.Api.DomainCore.Repository;

namespace Product.Api.Infrastructure.Repository
{
    public class WriteOnlyProductRepository : IWriteOnlyProductRepository
    {
        private readonly ProductContext _productContext;

        public WriteOnlyProductRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public async Task UpdateAsync()
        {
            await _productContext.SaveChangesAsync();
        }

        public async Task SaveProductAsync(DomainCore.Models.Product product)
        {
            await _productContext.AddAsync(product);
            await _productContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DomainCore.Models.Product product)
        {
            _productContext.Remove(product);
            await _productContext.SaveChangesAsync();
        }   
    }
}
