using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Repository.Contracts
{
    public interface ICategoryRepository
    {
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
        Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync();
        Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category);
        Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category);
        Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int id);
    }
}
