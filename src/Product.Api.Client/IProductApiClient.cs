using System.Threading.Tasks;
using Product.Api.Contract;
using Product.Api.Contract.Responses;

namespace Product.Api.Client
{
    public interface IProductApiClient
    {
        Task<ApiResponse<GetProductsResponse>> GetAll(string searchTerm);
        Task<ApiResponse<ProductDetailsResponse>> Get(int id);
        Task<ApiResponse<Response>> Delete(int id);
        Task<ApiResponse<ViewProduct>> Create(Contract.CreateProduct product);
        Task<ApiResponse<ViewProduct>> Update(Contract.CreateProduct product, int id);
        Task<ApiResponse<FileExport>> Export();
        Task<ApiResponse> IsCodeUnique(string code);
    }
}