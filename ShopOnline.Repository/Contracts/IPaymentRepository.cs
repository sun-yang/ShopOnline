using Microsoft.AspNetCore.Http;
using Stripe.Checkout;

namespace ShopOnline.Repository.Contracts
{
    public interface IPaymentRepository
    {
        Task<Session> CreateCheckoutSessionAsync();
        Task<ServiceResponse<bool>> FulfillOrderAsync(HttpRequest request);
    }
}
