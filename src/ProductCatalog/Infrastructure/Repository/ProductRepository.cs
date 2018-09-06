using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.Api.DomainCore.Repository;

namespace Product.Api.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _productContext;

        public ProductRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public async Task Update()
        {
            await _productContext.SaveChangesAsync();
        }

        public async Task SaveProduct(DomainCore.Models.Product product)
        {
            await _productContext.AddAsync(product);
            await _productContext.SaveChangesAsync();
        }

        public async Task<DomainCore.Models.Product> GetProductByCode(string code)
        {
            string productCode = code.Trim();
            return await _productContext.Product.Where(x => x.Code == productCode).FirstOrDefaultAsync();
        }

        public async Task Delete(DomainCore.Models.Product product)
        {
            _productContext.Remove(product);
            await _productContext.SaveChangesAsync();
        }

        public Task<bool> DoesProductExist(int id)
        {
            return _productContext.Product.Where(x => x.Id == id).AnyAsync();
        }

        public async Task<DomainCore.Models.Product> GetProduct(int id)
        {
            return await _productContext.Product.Include(x => x.Image).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<DomainCore.Models.Product>> GetProducts(string searchTerm)
        {
            var query = _productContext.Product.Include(x => x.Image);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                return await query.Where(x => x.Code.Contains(searchTerm) || x.Name.Contains(searchTerm)).ToListAsync();
            }

            return await query.ToListAsync();
        }

      
    }
}
