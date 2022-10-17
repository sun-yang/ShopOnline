using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Repository.Contracts
{
    public interface ICartRepository
    {
        Task<ServiceResponse<List<CartProductDTO>>> GetCartProductsAsync(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductDTO>>> StoreCartItemsAsync(List<CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemCountAsync();
        Task<ServiceResponse<List<CartProductDTO>>> GetDbCartProductsAsync(int? userId = null);
        Task<ServiceResponse<bool>> AddToCartAsync(CartItem cartItem);
        Task<ServiceResponse<bool>> UpdateQuantityAsync(CartItem cartItem);
        Task<ServiceResponse<bool>> RemoveItemFromCartAsync(int productId);
    }
}
