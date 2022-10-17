using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;


namespace ShopOnline.BlazorClient.Services
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService localStorage;
        private readonly HttpClient httpClient;
        private readonly IAuthService authService;

        public event Action CartChanged;
        public CartService(ILocalStorageService localStorage, HttpClient httpClient, IAuthService authService)
        {
            this.localStorage = localStorage;
            this.httpClient = httpClient;
            this.authService = authService;
        }

        public async Task AddToCartAsync(CartItem cartItem)
        {
            if (await authService.IsUserAuthenticatedAsync())
            {
                await httpClient.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                    cart = new List<CartItem>();

                var sameItem = cart.Find(i => i.ProductId == cartItem.ProductId);
                if (sameItem == null)
                    cart.Add(cartItem);
                else
                    sameItem.Quantity += cartItem.Quantity;

                await localStorage.SetItemAsync("cart", cart);
            }          

            await GetCartItemsCountAsync();
        }

     
        public async Task<List<CartProductDTO>> GetCartProductsAsync()
        {
            if(await authService.IsUserAuthenticatedAsync())
            {
                var response = await httpClient.GetFromJsonAsync<ServiceResponse<List<CartProductDTO>>>("api/cart");
                return response.Data;
            }
            else
            {
                var cartItems = await localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                    return new List<CartProductDTO>();
                var response = await httpClient.PostAsJsonAsync("api/cart/products", cartItems);
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductDTO>>>();
                return result.Data;
            }            
        }

        public async Task RemoveCartProductAsync(int productId)
        {
            if (await authService.IsUserAuthenticatedAsync())
            {
                await httpClient.DeleteAsync($"api/cart/{productId}");
            }
            else
            {
                var cart = await localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart != null)
                {
                    var cartItem = cart.Find(p => p.ProductId == productId);
                    if (cartItem != null)
                    {
                        cart.Remove(cartItem);
                        await localStorage.SetItemAsync("cart", cart);
                        CartChanged.Invoke();
                    }
                }
            }
        }

        public async Task UpdateQuantityAsync(CartProductDTO product)
        {
            if (await authService.IsUserAuthenticatedAsync())
            {
                var request = new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity
                };
                await httpClient.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await localStorage.GetItemAsync<List<CartItem>>("cart");
                var result = cart.Find(p => p.ProductId == product.ProductId);
                if (result != null)
                {
                    result.Quantity = product.Quantity;
                    await localStorage.SetItemAsync("cart", cart);
                }
            }                
        }

        public async Task StoreCartItemsAsync(bool emptyLocalCart)
        {
            var localCart = await localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)            
                return;
            
            await httpClient.PostAsJsonAsync("api/cart", localCart);

            if (emptyLocalCart)
            {
                await localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task GetCartItemsCountAsync()
        {
            if(await authService.IsUserAuthenticatedAsync())
            {
                var result = await httpClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;
                await localStorage.SetItemAsync("cartItemCount", count);
            }
            else
            {
                var localCart = await localStorage.GetItemAsync<List<CartItem>>("cart");
                await localStorage.SetItemAsync("cartItemCount", localCart == null ? 0: localCart.Count);
            }
            CartChanged.Invoke();
        }
    }
}
