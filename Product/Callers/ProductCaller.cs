using Microsoft.AspNetCore.Http;
using Product.Interface;
using Product.Model;

namespace Product.Callers
{
    public class ProductCaller : IProductCaller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductCaller(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ApiResponse<Products>> Add(Products products)
        {
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            var endpoint = $"{apiBaseUrl}/api/Product/Add";

            var httpResponse = await _httpClient.PostAsJsonAsync(endpoint, products);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new ApiResponse<Products>
                {
                    StatusCode = (int)httpResponse.StatusCode,
                    Error = httpResponse.ReasonPhrase
                };
            }

            // Deserialize your ApiResponse from backend API
            var data = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Products>>();

            return data;
        }

        public async Task<ApiResponse<Products>> Delete(string product_code)
        {
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            var endpoint = $"{apiBaseUrl}/api/Product/Delete/{product_code}";

            var httpResponse = await _httpClient.GetAsync(endpoint);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new ApiResponse<Products>
                {
                    StatusCode = (int)httpResponse.StatusCode,
                    Error = httpResponse.ReasonPhrase
                };
            }

            var wrapper = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Products>>();
            return wrapper;
        }

        public async Task<ApiResponse<Products>> Edit(Products products)
        {
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            var endpoint = $"{apiBaseUrl}/api/Product/Update";

            var httpResponse = await _httpClient.PostAsJsonAsync(endpoint, products);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new ApiResponse<Products>
                {
                    StatusCode = (int)httpResponse.StatusCode,
                    Error = httpResponse.ReasonPhrase
                };
            }

            // Deserialize your ApiResponse from backend API
            var data = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Products>>();

            return data;
        }

        public async Task<ApiResponse<Products>> Get(string product_code)
        {
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            var endpoint = $"{apiBaseUrl}/api/Product/GetByCode/{product_code}";

            var httpResponse = await _httpClient.GetAsync(endpoint);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new ApiResponse<Products>
                {
                    StatusCode = (int)httpResponse.StatusCode,
                    Error = httpResponse.ReasonPhrase
                };
            }

            var wrapper = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Products>>();
            return wrapper;
        }

        public async Task<ApiResponse<IList<Products>>> GetAll()
        {
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            var endpoint = $"{apiBaseUrl}/api/Product/GetAll";

            var httpResponse = await _httpClient.GetAsync(endpoint);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new ApiResponse<IList<Products>>
                {
                    StatusCode = (int)httpResponse.StatusCode,
                    Error = httpResponse.ReasonPhrase
                };
            }

            // Deserialize your ApiResponse from backend API
            var data = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<IList<Products>>>();

            return data;
        }
    }
}
