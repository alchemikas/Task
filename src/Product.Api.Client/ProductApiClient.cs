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

        public ProductApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:59266")
            };
        }

        public async Task<GetProductsResponse> GetAll(string searchTerm)
        {
            var search = string.IsNullOrEmpty(searchTerm) ? string.Empty : searchTerm;
            HttpClient httpClient = GetHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"api/product?searchterm={search}");
            return await HandleResponse<GetProductsResponse>(response, $"Failed to retrieve products from api.");
        }

        public async Task<GetProductDetailsResponse> Get(int id)
        {
            HttpClient httpClient = GetHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"api/product/{id}");
            return await HandleResponse<GetProductDetailsResponse>(response, $"Failed to retrieve product from api.");
        }

        public async Task Delete(int id)
        {
            HttpClient httpClient = GetHttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/product/{id}");
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            throw new Exception($"Failed to retrieve product from api.");
        }

        public async Task<Response> Create(Contract.CreateProduct product)
        {
            HttpClient httpClient = GetHttpClient();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(product));
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = await httpClient.PostAsync($"api/product", content);
            if (response.IsSuccessStatusCode)
            {
                return new Response();
            }
            return await HandleResponse<Response>(response, $"Failed to create resource");
        }
    

        public async Task Update(Contract.CreateProduct product)
        {
            HttpClient httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            HttpContent content = new StringContent(JsonConvert.SerializeObject(product));
            HttpResponseMessage response = await httpClient.PutAsync($"api/product", content);
//            return await HandleResponse<Response>(response, $"Failed to create resource");
        }

        public void Export()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.ms-excel."));
//            return await HandleResponse<Response>(response, $"Failed to create resource");
        }

        private HttpClient GetHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return _httpClient;
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response, string errorMessage) 
        {
            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                return await response.Content.DeserializeResponse<T>();
            }
            throw new Exception(errorMessage);
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
