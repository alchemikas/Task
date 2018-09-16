using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.Api.DomainCore.Repository;

namespace Product.Api.Infrastructure.Repository
{
    public class ReadOnlyProductRepository : IReadOnlyProductRepository
    {
        private readonly ProductContext _productContext;

        public ReadOnlyProductRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public async Task<DomainCore.Models.Product> GetProductByCodeAsync(string code)
        {
            string productCode = code.Trim();
            return await _productContext.Product.Where(x => x.Code == productCode).FirstOrDefaultAsync();
        }

   
        public Task<bool> DoesProductExistAsync(int id)
        {
            return _productContext.Product.Where(x => x.Id == id).AnyAsync();
        }

        public async Task<DomainCore.Models.Product> GetProductAsync(int id)
        {
            return await _productContext.Product.Include(x => x.Image).Include(x => x.ImageThumbnail).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<DomainCore.Models.Product>> GetProductsAsync(string searchTerm)
        {
            var query = _productContext.Product.Include(x => x.ImageThumbnail);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                return await query.Where(x => x.Code.Contains(searchTerm) || x.Name.Contains(searchTerm)).ToListAsync();
            }

            return await query.ToListAsync();
        }


    }
}