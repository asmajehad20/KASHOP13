using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using KASHOP13.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;

using Stripe.Checkout;
using System.Security.Cryptography;

namespace KASHOP13.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IEmailSender _emailSender;

        public CheckoutService(
            ICartRepository cartRepository, 
            UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor httpContextAccessor, 
            IOrderRepository orderRepository, 
            ICartService cartService,
            IProductRepository productRepository,
            IEmailSender emailSender)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _cartService = cartService;
            _productRepository = productRepository;
            _emailSender = emailSender;
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
                    SuccessUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/checkouts/success?sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/checkouts/cancel",

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

                order.StripeSessionId = session.Id;
                await _orderRepository.UpdateAsync(order);

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

        public async Task<CheckoutResponse> HandleSuccess(string sessionId)
        {
            var order = await _orderRepository.GetOne(
                o => o.StripeSessionId == sessionId,
                includes: new[]
                {
                    nameof(Order.Items),
                    $"{nameof(Order.Items)}.{nameof(OrderItem.Product)}",
                    $"{nameof(Order.Items)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"
                }
                );
            order.OrderStatus = OrderStatusEnum.Paid;

            await _orderRepository.UpdateAsync(order);

            await _cartService.ClearCart(order.UserId);

            var user = await _userManager.FindByIdAsync(order.UserId);
            await _emailSender.SendEmailAsync(user.Email, "order confirm", "<h2>your order has been placed successfully</h2>");

            var lowStockProducts = await _productRepository.DecreaseQuantityAsync(order.Items);

            if(lowStockProducts != null)
            {
                foreach(var product in lowStockProducts)
                {
                    await _emailSender.SendEmailAsync("asmajehad919@gmail.com", "stock notice", $"<p>stock is low, less tha 5. product ({product.Translations.FirstOrDefault(t => t.Language == "en").Name}) current quantity : {product.Quantity}</p>");
                }
                
            }

            return new CheckoutResponse
            {
                Success = true,
                OrderId = order.Id
            };

        }


    }
}
