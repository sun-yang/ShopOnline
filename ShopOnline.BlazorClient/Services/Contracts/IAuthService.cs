namespace ShopOnline.BlazorClient.Services.Contracts
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> RegiterAsync(UserRegister request);
        Task<ServiceResponse<string>> LoginAsync(UserLogin request);
        Task<ServiceResponse<bool>> ChangePasswordAsync(UserChangePassword request);
        Task<bool> IsUserAuthenticatedAsync();
    }
}
