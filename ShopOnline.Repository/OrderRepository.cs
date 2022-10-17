using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext dbContext;
        private readonly ICartRepository cartRepository;
        private readonly IAuthRepository authRepository;

        public OrderRepository(DataContext dbContext, ICartRepository cartRepository, IAuthRepository authRepository)
        {
            this.dbContext = dbContext;
            this.cartRepository = cartRepository;
            this.authRepository = authRepository;
        }

        public async Task<ServiceResponse<OrderDetailsDTO>> GetOrderDetailsAsync(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsDTO>();
            var order = await dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == authRepository.GetUserId() &&
                o.OrderId == orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            if(order == null)
            {
                response.Success = false;
                response.Message = "Order not found.";
                return response;
            }
            response.Data = new OrderDetailsDTO
            {
                OrderDate = order.OrderDate,
                OrderPrice = order.OrderPrice,
                Products = new List<OrderDetailsProductDTO>()
            };

            order.OrderItems.ForEach(i =>
            response.Data.Products.Add(new OrderDetailsProductDTO
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                TotalPrice = i.TotalPrice,
                ImageUrl = i.Product.ImageUrl,
                Quantity = i.Quantity
            }));

            return response;                
        }

        public async Task<ServiceResponse<List<OrderDTO>>> GetOrdersAsync()
        {
            var response = new ServiceResponse<List<OrderDTO>>();
            var orders = await dbContext.Orders
                .Include( o => o.OrderItems)
                .ThenInclude( oi => oi.Product)
                .Where( o => o.UserId == authRepository.GetUserId())
                .OrderByDescending( o => o.OrderDate)
                .ToListAsync();

            var orderDtos = new List<OrderDTO>();
            orders.ForEach(o => orderDtos.Add(new OrderDTO
            {
                Id = o.OrderId,
                OrderPrice = o.OrderPrice,
                OrderDate = o.OrderDate,
                ProductName = o.OrderItems.Count > 1 ? 
                $"{o.OrderItems.First().Product.Name}" +
                $" and {o.OrderItems.Count - 1} more..." :
                o.OrderItems.First().Product.Name,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl
            }));

            response.Data = orderDtos;
            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrderAsync(int userId)
        {
            var products = (await cartRepository.GetDbCartProductsAsync(userId)).Data;
            decimal totalPrice = 0;
            products.ForEach(p => totalPrice += p.ProductPrice * p.Quantity);

            var orderItems = new List<OrderItem>();

            products.ForEach(p => orderItems.Add(new OrderItem
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                TotalPrice = p.ProductPrice * p.Quantity
            }));

            var order = new Order
            {
                UserId = userId,
                OrderItems = orderItems,
                OrderDate = DateTime.Now,
                OrderPrice = totalPrice
            };

            dbContext.Orders.Add(order);
            dbContext.CartItems.RemoveRange(dbContext.CartItems.Where(ci => ci.UserId == userId));
            await dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
