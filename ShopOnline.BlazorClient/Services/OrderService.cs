namespace ShopOnline.BlazorClient.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient httpClient;
        private readonly IAuthService authService;

        public OrderService(HttpClient httpClient, IAuthService authService)
        {
            this.httpClient = httpClient;
            this.authService = authService;
        }

        public async Task<OrderDetailsDTO> GetOrderDetailsAsync(int orderId)
        {
            var response = await httpClient.GetFromJsonAsync<ServiceResponse<OrderDetailsDTO>>($"api/order/{orderId}");
            return response.Data;
        }

        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            var response = await httpClient.GetFromJsonAsync<ServiceResponse<List<OrderDTO>>>("api/order");
            return response.Data;
        }

        public async Task<string> PlaceOrderAsync()
        {
            if (await authService.IsUserAuthenticatedAsync())
            {
                var result = await httpClient.PostAsync("api/payment/checkout", null);
                var url = await result.Content.ReadAsStringAsync();
                return url;
            }
            else
            {
                return "login";
            }
        }
    }
}
