namespace ShopOnline.BlazorClient.Services.Contracts
{
    public interface ICartService
    {
        event Action CartChanged;
        Task AddToCartAsync(CartItem cartItem);       
        Task<List<CartProductDTO>> GetCartProductsAsync();
        Task RemoveCartProductAsync(int productId);
        Task UpdateQuantityAsync(CartProductDTO product);
        Task StoreCartItemsAsync(bool emptyLocalCart);
        Task GetCartItemsCountAsync();

    }
}
