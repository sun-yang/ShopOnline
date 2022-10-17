namespace ShopOnline.BlazorClient.Services.Contracts
{
    public interface IProductService
    {
        List<Product> Products { get; set; }
        List<Product> AdminProducts { get; set; }

        event Action ProductChanged;
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchString { get; set; }
        Task GetProductsAsync(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductAsync(int id);
        Task SearchProductsAsync(string searchString, int page);
        Task<List<string>> GetProductSearchSuggestionsAysnc(string searchString);
        Task GetAdminProductsAsync();
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);

    }
}
