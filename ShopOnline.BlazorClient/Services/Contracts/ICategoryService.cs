namespace ShopOnline.BlazorClient.Services.Contracts
{
    public interface ICategoryService
    {
        event Action CategoryChanged;
        public List<Category> Categories { get; set; }
        public List<Category> AdminCategories { get; set; }
        Task GetCategoriesAsync();
        Task GetAdminCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);
        Category CreateNewCategory();
    }
}
