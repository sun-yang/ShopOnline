namespace ShopOnline.Repository.Contracts
{
    public interface IProductRepository
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductAsync(int id);
        Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
        Task<ServiceResponse<ProductSearchResultDTO>> SearchProductsAsync(string searchString, int page);
        Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchString);
        Task<ServiceResponse<List<Product>>> GetAdminProductsAsync();
        Task<ServiceResponse<Product>> CreateProductAsync(Product product);
        Task<ServiceResponse<Product>> UpdateProductAsync(Product product);
        Task<ServiceResponse<bool>> DeleteProductAsync(int productId);
    }
}
