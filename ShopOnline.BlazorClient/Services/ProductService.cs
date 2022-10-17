using ShopOnline.BlazorClient.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.BlazorClient.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public event Action ProductChanged;
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Product> AdminProducts { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading...";
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string LastSearchString { get; set; }

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {              
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{id}");
            return result;
        }

        public async Task GetProductsAsync(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
             await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") :
             await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            if (result != null && result.Data != null)
                Products = result.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
                Message = "No products found.";

            ProductChanged.Invoke();
        }

        public async Task SearchProductsAsync(string searchString, int page)
        {
            LastSearchString = searchString;
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<ProductSearchResultDTO>>($"api/product/search/{searchString}/{page}");
            if(result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.TotalPages;
            }
               
            if(Products.Count == 0)
            {
                Message = "No products found.";
            }
            ProductChanged?.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestionsAysnc(string searchString)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchString}");
            return result.Data;
        }

        public async Task GetAdminProductsAsync()
        {
            var result = await httpClient
              .GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
            AdminProducts = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts.Count == 0)
                Message = "No products found.";
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var result = await httpClient.PostAsJsonAsync("api/product", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
            return newProduct;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var result = await httpClient.PutAsJsonAsync($"api/product", product);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>();
            return content.Data;
        }

        public async Task DeleteProductAsync(Product product)
        {
            var result = await httpClient.DeleteAsync($"api/product/{product.Id}");
        }
    }
}
