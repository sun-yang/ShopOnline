using System.Net.Http.Json;

namespace ShopOnline.BlazorClient.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient; 

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Category> AdminCategories { get; set; } = new List<Category>();

        public event Action CategoryChanged;
        public CategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task AddCategoryAsync(Category category)
        {
            var result = await httpClient.PostAsJsonAsync("api/category/admin", category);
            AdminCategories = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategoriesAsync();
            CategoryChanged.Invoke();
        }

        public Category CreateNewCategory()
        {
            var newCategory = new Category { IsNew = true, Editing = true };
            AdminCategories.Add(newCategory);
            CategoryChanged.Invoke();
            return newCategory;
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var result = await httpClient.DeleteAsync($"api/category/admin/{categoryId}");
            AdminCategories = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategoriesAsync();
            CategoryChanged.Invoke();
        }

        public async Task GetAdminCategoriesAsync()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category/admin");
            if (result != null && result.Data != null)
                AdminCategories = result.Data;
        }

        public async Task GetCategoriesAsync()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if(result != null && result.Data != null)
                Categories = result.Data;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var result = await httpClient.PutAsJsonAsync("api/category/admin", category);
            AdminCategories = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategoriesAsync();
            CategoryChanged.Invoke();
        }
    }
}
