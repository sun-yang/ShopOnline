namespace ShopOnline.BlazorClient.Services.Contracts
{
    public interface IOrderService
    {
        Task<string> PlaceOrderAsync();
        Task<List<OrderDTO>> GetOrdersAsync();
        Task<OrderDetailsDTO> GetOrderDetailsAsync(int orderId);
    }
}
