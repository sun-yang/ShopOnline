using Microsoft.AspNetCore.Components.Authorization;

namespace ShopOnline.BlazorClient.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
        }

        public async Task<ServiceResponse<bool>> ChangePasswordAsync(UserChangePassword request)
        {
            var result = await httpClient.PostAsJsonAsync("api/auth/change-password", request.Password);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<string>> LoginAsync(UserLogin request)
        {
            var result = await httpClient.PostAsJsonAsync("api/auth/login", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> RegiterAsync(UserRegister request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();             
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            return (await authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
