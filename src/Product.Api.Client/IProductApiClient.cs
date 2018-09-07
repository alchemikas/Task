using System.Threading.Tasks;
using Product.Api.Contract;
using Product.Api.Contract.Responses;

namespace Product.Api.Client
{
    public interface IProductApiClient
    {
        Task<GetProductsResponse> GetAll(string searchTerm);
        Task<GetProductDetailsResponse> Get(int id);
        Task Delete(int id);
        Task<Response> Create(Contract.CreateProduct product);
        Task<Response> Update(Contract.CreateProduct product, int id);
        Task<FileExport> Export();
        Task<bool> IsCodeUnique(string code);
    }
}