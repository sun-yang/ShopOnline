using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Repository.Contracts
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> RegisterAsync(User user, string password);
        Task<bool> IsUserExistAsync(string email);
        Task<ServiceResponse<string>> LoginAsync(string email, string password);
        Task<ServiceResponse<bool>> ChangePasswordAsync(int userId, string newPassword);
        int GetUserId();
        string GetUserEmail();
        Task<User> GetUserByEmailAsync(string email);     

    }
}
