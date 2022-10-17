using Microsoft.AspNetCore.Http;
using Stripe;
using Stripe.Checkout;

namespace ShopOnline.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ICartRepository cartRepository;
        private readonly IAuthRepository authRepository;
        private readonly IOrderRepository orderRepository;
        const string secretKey = "whsec_66ec5311701db7129a48dd28a56034ea903d59affe20e803c7644bdfba0e351d";

        public PaymentRepository(ICartRepository cartRepository,
            IAuthRepository authRepository,
            IOrderRepository orderRepository)
        {
            this.cartRepository = cartRepository;
            this.authRepository = authRepository;
            this.orderRepository = orderRepository;

            StripeConfiguration.ApiKey = "sk_test_51Lrv6IIR5y0JokdLHVVJE9LL764kqPsK3IsT4AruHcgodb2HkcbX8vyyH3DesreL66lUpvye0ug43EjtfnUXmajD005F0Kor5X";
        }
        public async Task<Session> CreateCheckoutSessionAsync()
        {
            var products = (await cartRepository.GetDbCartProductsAsync()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.ProductPrice * 100,
                    Currency = "aud",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.ProductName,
                        Images = new List<string> { product.ProductImageUrl }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = authRepository.GetUserEmail(),
                ShippingAddressCollection =
                    new SessionShippingAddressCollectionOptions
                    {
                        AllowedCountries = new List<string> { "AU" }
                    },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7296/order-success",
                CancelUrl = "https://localhost:7296/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FulfillOrderAsync(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        secretKey
                    );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await authRepository.GetUserByEmailAsync(session.CustomerEmail);
                    await orderRepository.PlaceOrderAsync(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };
            }
        }
    
    }
}
