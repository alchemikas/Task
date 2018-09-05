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

        public async Task SaveProduct(Models.Product product)
        {
            await _productContext.AddAsync(product);
            await _productContext.SaveChangesAsync();
        }

        public async Task<Models.Product> GetProductByCode(string code)
        {
            string productCode = code.Trim();
            return await _productContext.Product.Where(x => x.Code == productCode).FirstOrDefaultAsync();
        }

        public async Task Delete(Models.Product product)
        {
            _productContext.Remove(product);
            await _productContext.SaveChangesAsync();
        }

        public async Task<Models.Product> GetProduct(int id)
        {
            return await _productContext.Product.Include(x => x.Image).FirstOrDefaultAsync(x => x.Id == id);
        }


        Task<List<Models.Product>> IProductRepository.GetProducts(string searchTerm)
        {
            var query = _productContext.Product.Include(x => x.Image);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                return query.Where(x => x.Code.Contains(searchTerm) || x.Name.Contains(searchTerm)).ToListAsync();
            }

            return query.ToListAsync();
        }

        Task<Models.Product> IProductRepository.GetProduct(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
