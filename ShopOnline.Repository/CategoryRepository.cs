using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext dbContext;

        public CategoryRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category)
        {
            category.Editing = category.IsNew = false;
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            return await GetAdminCategoriesAsync();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int id)
        {
            Category category = await GetCategoryByIdAsync(id);
            if (category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            category.Deleted = true;
            await dbContext.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
        {
            var categories = await dbContext.Categories
               .Where(c => !c.Deleted)
               .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var result = await dbContext.Categories
                .Where( c => !c.Deleted && c.Visible)
                .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = result
            };
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category)
        {
            var dbCategory = await GetCategoryByIdAsync(category.Id);
            if (dbCategory == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await dbContext.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        private async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
