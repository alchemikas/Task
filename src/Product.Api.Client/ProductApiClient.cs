using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Product.Api.Contract;
using Product.Api.Contract.Responses;

namespace Product.Api.Client
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _httpClient;
        private const string API_NOT_ACCESSIBLE = "Product API not accessible.";

        public ProductApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8000")
            };
        }

        public async Task<ApiResponse<GetProductsResponse>> GetAll(string searchTerm)
        {
            var search = string.IsNullOrEmpty(searchTerm) ? string.Empty : searchTerm;

            HttpClient httpClient = GetHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"api/product?searchterm={search}");

            var apiResponse = new ApiResponse<GetProductsResponse>()
            {
                HttpStatusCode = response.StatusCode,
                Response = await HandleResponse<GetProductsResponse>(response)
            };
            return apiResponse;
        }

        public async Task<ApiResponse<ProductDetailsResponse>> Get(int id)
        {
            HttpClient httpClient = GetHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"api/product/{id}");
            var apiResponse = new ApiResponse<ProductDetailsResponse>()
            {
                HttpStatusCode = response.StatusCode,
                Response = await HandleResponse<ProductDetailsResponse>(response)
            };
            return apiResponse;
        }

        public async Task<ApiResponse<Response>> Delete(int id)
        {
            HttpClient httpClient = GetHttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/product/{id}");
            var apiResponse = new ApiResponse<Response>()
            {
                HttpStatusCode = response.StatusCode,
                Response = await HandleResponse<Response>(response)
            };

            return apiResponse;
        }

        public async Task<ApiResponse<ViewProduct>> Create(Contract.CreateProduct product)
        {
            HttpClient httpClient = GetHttpClient();

            HttpContent content = new StringContent(JsonConvert.SerializeObject(product));
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = await httpClient.PostAsync($"api/product", content);
            var apiResponse = new ApiResponse<ViewProduct>
            {
                HttpStatusCode = response.StatusCode,
                Response = await HandleResponse<ViewProduct>(response)
            };
            return apiResponse;
        }
    

        public async Task<ApiResponse<ViewProduct>> Update(Contract.CreateProduct product, int id)
        {
            HttpClient httpClient = GetHttpClient();
            
            HttpContent content = new StringContent(JsonConvert.SerializeObject(product));
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = await httpClient.PutAsync($"api/product/{id}", content);
            var apiRresponse = new ApiResponse<ViewProduct>()
            {
                 HttpStatusCode = response.StatusCode,
                 Response = await HandleResponse<ViewProduct>(response)
            };
           
            return apiRresponse;
        }

        public async Task<ApiResponse<FileExport>> Export()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.ms-excel."));
            HttpResponseMessage response = await _httpClient.GetAsync("api/product");
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<FileExport>
                {
                    Response = new FileExport
                    {
                        Bytes = await response.Content.ReadAsByteArrayAsync(),
                        FileName = response.Content.Headers.ContentDisposition.FileNameStar,
                        ContentType = response.Content.Headers.ContentType.MediaType
                    },
                   HttpStatusCode = response.StatusCode
                };
            }
            throw new Exception(API_NOT_ACCESSIBLE);
        }

        public async Task<ApiResponse> IsCodeUnique(string code)
        {
            HttpClient httpClient = GetHttpClient();
            var requestMessage= new HttpRequestMessage(HttpMethod.Head, $"api/product/code/{code}");
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

            return new ApiResponse(){HttpStatusCode = response.StatusCode };
        }


        private HttpClient GetHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return _httpClient;
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response) 
        {
            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                return await response.Content.DeserializeResponse<T>();
            }
            throw new Exception(API_NOT_ACCESSIBLE);
        }
    }

    public static class DeserializationExtension
    {
        public static async Task<T> DeserializeResponse<T>(this HttpContent content)
        {
            string responseContent = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
