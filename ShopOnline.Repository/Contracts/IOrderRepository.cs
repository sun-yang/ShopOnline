using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Repository.Contracts
{
    public interface IOrderRepository
    {
        Task<ServiceResponse<bool>> PlaceOrderAsync(int userId);
        Task<ServiceResponse<List<OrderDTO>>> GetOrdersAsync();
        Task<ServiceResponse<OrderDetailsDTO>> GetOrderDetailsAsync(int orderId);
    }
}
