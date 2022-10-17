using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ShopOnline.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext dbContext;
        private readonly IAuthRepository authRepository;

        public CartRepository(DataContext dbContext, IAuthRepository authRepository)
        {
            this.dbContext = dbContext;
            this.authRepository = authRepository;
        }
        public async Task<ServiceResponse<List<CartProductDTO>>> GetCartProductsAsync(List<CartItem> cartItems)
        {
            var response = new ServiceResponse<List<CartProductDTO>>
            {
                Data = new List<CartProductDTO>()
            };

            foreach (var cartItem in cartItems)
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);
                if (product == null)
                    continue;
                var cartProduct = new CartProductDTO
                {
                    ProductId = product.Id,
                    ProductImageUrl = product.ImageUrl,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = cartItem.Quantity
                };
                response.Data.Add(cartProduct);
            }

            return response;

        }

       
        public async Task<ServiceResponse<List<CartProductDTO>>> StoreCartItemsAsync(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = authRepository.GetUserId());
            dbContext.CartItems.AddRange(cartItems);
            await dbContext.SaveChangesAsync();

            return await GetCartProductsAsync(
                await dbContext.CartItems.Where(ci => ci.UserId == authRepository.GetUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<int>> GetCartItemCountAsync()
        {
            var count = (await dbContext.CartItems.Where(ci => ci.UserId == authRepository.GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductDTO>>> GetDbCartProductsAsync(int? userId = null)
        {
            if (userId == null)
                userId = authRepository.GetUserId();

            return await GetCartProductsAsync(await dbContext.CartItems
                .Where(ci => ci.UserId == userId).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCartAsync(CartItem cartItem)
        {
            cartItem.UserId = authRepository.GetUserId();

            var sameItem = await dbContext.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                ci.UserId == cartItem.UserId);
            if (sameItem == null)
            {
                dbContext.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantityAsync(CartItem cartItem)
        {
            var dbCartItem = await dbContext.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                ci.UserId == authRepository.GetUserId());
            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCartAsync(int productId)
        {
            var dbCartItem = await dbContext.CartItems
               .FirstOrDefaultAsync(ci => ci.ProductId == productId &&
               ci.UserId == authRepository.GetUserId());
            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };
            }

            dbContext.CartItems.Remove(dbCartItem);
            await dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
