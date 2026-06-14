using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using KASHOP13.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Stripe.Checkout;

namespace KASHOP13.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;

        public CheckoutService(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
        }

        public async Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request)
        {
            var cartItems = await _cartRepository.GetAllAsync(
                filter: c => c.UserId == userId,
                includes: new[]
                {
                    nameof(Cart.Product),
                    $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"
                });

            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "cart is empty"
                };
            }

            var user = await _userManager.FindByIdAsync( userId );
            var city = request.City ?? user.City;
            if(city is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "city is required"
                };
            }
            var street = request.Street ?? user.Street;
            if (street is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "street is required"
                };
            }
            var phoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            if (phoneNumber is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "phoneNumber is required"
                };
            }

            foreach(var item in cartItems )
            {
                if(item.Count > item.Product.Quantity)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = "not enough in stock"
                    };
                }
            }


            var order = new Order()
            {
                UserId = userId,
                City = city,
                Street = street,
                PhoneNumber = phoneNumber,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = cartItems.Sum(c => c.Product.Price * c.Count),
                Items = cartItems.Select( c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Count,
                    UnitPrice = c.Product.Price,
                    TotalPrice = c.Product.Price * c.Count
                }).ToList(),
            };
            await _orderRepository.CreateAsync(order);

            if(request.PaymentMethod == PaymentMethodEnum.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true,
                };
            }

            if (request.PaymentMethod == PaymentMethodEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    SuccessUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/checkouts/success",
                    CancelUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/checkouts/cancel",

                    LineItems = new List<SessionLineItemOptions>()
                    {

                    }
                };

                foreach (var item in cartItems)
                {
                    options.LineItems.Add(
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "USD",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en").Name,
                                },
                                UnitAmount = (long)(item.Product.Price * 100),
                            },
                            Quantity = item.Count,
                        });
                }

                var service = new SessionService();
                var session = service.Create(options);

                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url,
                };
            }

            return new CheckoutResponse
            {
                Success = false,
                Error = "invalid payment method"
            };
        }
                
    }
}
