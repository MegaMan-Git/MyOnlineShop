using Application.Dtos.Cart.Cartitem;
using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Dtos.Payment;
using Application.Filters;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        #region DI
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork,
            IPaymentGateway paymentGateway,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Get Order&OrderItem For Admin
        public async Task<ServiceResult<IEnumerable<AdminOrderDto>>> GetAllOrdersForAdminAsync
            (QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<AdminOrderDto>>();

            var orders = await _unitOfWork.orderRepository.GetAllOrdersForAdminAsync(parameters);

            if (!orders.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".هیچ سفارشی در حال حاضر ثبت نشده است";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = orders;

            return result;
        }

        public async Task<ServiceResult<IEnumerable<AdminOrderItemDto>>> GetAllOrderItemsForAdminAsync
            (QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<AdminOrderItemDto>>();

            var orderItems = await _unitOfWork.orderRepository
                .GetAllOrderItemsForAdminAsync(parameters);

            if (!orderItems.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".هیچ سفارشی در حال حاضر ثبت نشده است";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = orderItems;

            return result;
        }
        #endregion

        #region Get Order&OrderItem
        public async Task<ServiceResult<IEnumerable<CustomerOrderDto>>> GetOrdersAsync
            (string userId, QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<CustomerOrderDto>>();

            var customerOrders = await _unitOfWork.orderRepository
                .GetCustomerOrdersAsync(userId, parameters);

            if (!customerOrders.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".در حال حاضر هنوز هیچ سفارشی ثبت نشده است";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = customerOrders;

            return result;
        }

        public async Task<ServiceResult<CustomerOrderItemDto>> GetOrderItemAsync
            (string userId, int orderItemId, int orderId)
        {
            var result = new ServiceResult<CustomerOrderItemDto>();

            //validate request
            var order = await _unitOfWork.orderRepository.GetOrderAsync(orderId, userId);
            if (order is null)
            {
                result.StatusCode = ResultStatusCode.Forbidden;
                result.Message = "سفارش یافت نشد یا دسترسی به آن امکان پذیر نیست.";

                return result;
            }

            var orderItemDto = await _unitOfWork.orderRepository
                .GetOrderItemAsync(orderItemId, order.Id);
            
            if (orderItemDto is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "آیتمی برای این سفارش یافت نشد.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = orderItemDto;

            return result;
        }

        public async Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> GetOrderItemsAsync
            (string userId, int orderId, QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<CustomerOrderItemDto>>();

            //validate request
            var order = await _unitOfWork.orderRepository.GetOrderAsync(orderId, userId);
            if (order is null)
            {
                result.StatusCode = ResultStatusCode.Forbidden;
                result.Message = "سفارش یافت نشد یا دسترسی به آن امکان پذیر نیست.";

                return result;
            }

            var orderItemsDto = await _unitOfWork.orderRepository
                .GetOrderItemsAsync(order.Id, parameters);
            
            if (!orderItemsDto.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "آیتمی برای این سفارش یافت نشد.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = orderItemsDto;

            return result;
        }
        #endregion

        #region Get Payment
        public async Task<ServiceResult<PaymentDto>> GetPaymentAsync(string userId, int orderId)
        {
            var result = new ServiceResult<PaymentDto>();

            //validate request
            var order = await _unitOfWork.orderRepository.GetOrderAsync(orderId, userId);
            if (order is null)
            {
                result.StatusCode = ResultStatusCode.Forbidden;
                result.Message = ".سفارش یافت نشد یا دسترسی به آن امکان پذیر نیست";

                return result;
            }

            var payment = await _unitOfWork.orderRepository.GetPaymentAsync(order.Id);
            if (payment is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".پرداختی برای این سفارش ثبت نشده";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<PaymentDto>(payment);

            return result;
        }

        public async Task<ServiceResult<IEnumerable<PaymentDto>>> GetPaymentsAsync
            (string userId, QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<PaymentDto>>();

            var payments = await _unitOfWork.orderRepository.GetPaymentsAsync(userId, parameters);
            if (!payments.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "در حال حاضر هیچ تراکنشی برای شما ثبت نشده.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<IEnumerable<PaymentDto>>(payments);

            return result;
        }
        #endregion

        #region Add OrderItem
        public async Task<ServiceResult<CustomerOrderItemDto>> CreateOrderFromCartItemAsync
            (string userId, int cartItemId)
        {
            var result = new ServiceResult<CustomerOrderItemDto>();

            //get cart and caritem
            var cart = await _unitOfWork.cartRepository.GetCartAsync(userId);
            if (cart is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".سبد خرید یافت نشد";

                return result;
            }
            var cartItem = await _unitOfWork.cartRepository.GetCartItemAsync(cartItemId, cart.Id);
            if (cartItem is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".آیتمی با شناسه ارسالی برای شما یافت نشد";

                return result;
            }


            //check product stock
            var product = await _unitOfWork.productRepository.GetProductByIdAsync(cartItem.ProductId);
            if (product is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".محصول از قبل انتخاب شده دیگر در فروشگاه وجود ندارد";

                return result;
            }
            if (product.Stock < cartItem.Quantity)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".تعداد سفارش درخواستی بیشتر از موجودی فعلی انبار میباشد! لطفا سفارش خود را اصلاح کنید";

                return result;
            }

            ////change product stock
            //product.Stock -= cartItem.Quantity;
            //await _unitOfWork.productRepository.UpdateProductAsync(product);

            //add new order
            var newOrder = new Order
            {
                UserId = userId
            };

            //add orderitem
            newOrder.OrderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = cartItem.Quantity,
                UnitPrice = product.Price
            });

            //add payment
            newOrder.Payment = new Payment
            {
                Amount = product.Price * cartItem.Quantity
            };

            //delete cartitem
            await _unitOfWork.cartRepository.DeleteCartItemAsync(cartItem);

            //add order & orderitem & payment
            await _unitOfWork.orderRepository.AddOrderAsync(newOrder);

            //savechanges
            await _unitOfWork.SaveChangesAsync();

            //return status
            result.StatusCode = ResultStatusCode.Success;
            result.Data = await _unitOfWork.orderRepository.GetOrderItemAsync(newOrder.OrderItems.Single().Id, newOrder.Id);

            return result;
        }

        public async Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> CreateOrderFromCartItemsAsync
            (string userId)
        {
            var result = new ServiceResult<IEnumerable<CustomerOrderItemDto>>();
            var products = new Dictionary<int, Product>();
            int totalPrice = 0;

            //get cart and cartitem dtos
            var cart = await _unitOfWork.cartRepository.GetCartAsync(userId);
            if (cart is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".سبد خرید یافت نشد";

                return result;
            }
            var cartItemDtos = await _unitOfWork.cartRepository.GetCustomerCartItemsAsync(cart.Id);
            if (!cartItemDtos.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".هیچ محصولی در سبد خرید شما وجود ندارد";

                return result;
            }


            //validate products
            foreach (var item in cartItemDtos)
            {
                var product = await _unitOfWork.productRepository.GetProductByIdAsync(item.ProductId);

                if (product is null)
                {
                    result.StatusCode = ResultStatusCode.BadRequest;
                    result.Errors.Add(
                        $"محصول از قبل انتخاب شده {item.ProductName} .دیگر در فروشگاه وجود ندارد");
                    continue;
                }

                products.Add(product.Id, product);

                if (product.Stock < item.Quantity)
                {
                    result.StatusCode = ResultStatusCode.BadRequest;
                    result.Errors.Add(
                        $"تعداد سفارش درخواستی محصول {item.ProductName} .بیشتر از موجودی انبار است");
                }
            }
            //if there are problems exist
            if (result.StatusCode == ResultStatusCode.BadRequest)
            {
                result.Message = ".مشکلی در هنگام ثبت سفارش پیش آمده";

                return result;
            }


            //add new order record in database
            var newOrder = new Order
            {
                UserId = userId
            };


            foreach (var item in cartItemDtos)
            {
                var product = products[item.ProductId];

                ////change product stock
                //product.Stock -= item.Quantity;
                //await _unitOfWork.productRepository.UpdateProductAsync(product);

                //add orderitem
                newOrder.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                totalPrice += product.Price * item.Quantity;
            }

            //add payment
            newOrder.Payment = new Payment
            {
                Amount = totalPrice
            };

            //delete cartitems
            var cartItems = await _unitOfWork.cartRepository.GetCartItemsAsync(cart.Id); 
            await _unitOfWork.cartRepository.DeleteCartItemsAsync(cartItems);
            

            //add order & orderitems & payment
            await _unitOfWork.orderRepository.AddOrderAsync(newOrder);

            //savechanges
            await _unitOfWork.SaveChangesAsync();

            //return status
            result.StatusCode = ResultStatusCode.Success;
            result.Data = await _unitOfWork.orderRepository.GetOrderItemsAsync(newOrder.Id);

            return result;
        }
        #endregion

        #region Payment Process
        public async Task<ServiceResult<string>> StartPaymentAsync
            (string userId, int orderId, string callbackUrl)
        {
            var result = new ServiceResult<string>();

            var order = await _unitOfWork.orderRepository.GetOrderAsync(orderId, userId);

            if (order is null)
            {
                _logger.LogWarning(
                    "Payment start failed because OrderId {OrderId} was not found for UserId {UserId}",
                    orderId,
                    userId);

                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".سفارش یافت نشد";

                return result;
            }

            var payment = await _unitOfWork.orderRepository.GetPaymentAsync(orderId);
            if (payment is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".پرداخت یافت نشد";

                return result;
            }

            if(payment.Status != PaymentStatus.Waiting.ToString())
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message =
                    ".وضعیت سفارش تکمیل یا منقضی شده است و دیگر امکان پرداخت آن وجود ندارد";

                return result;
            }

            #region Check Product IsDeleted
            var orderItems = await _unitOfWork.orderRepository.GetOrderItemsIncludeProductsAsync(order.Id);

            foreach (var item in orderItems)
            {
                if (item.Product is null)
                    throw new InvalidOperationException(".محصول سفارش یافت نشد");

                if (item.Product.IsDeleted is true)
                {
                    result.StatusCode = ResultStatusCode.NotFound;
                    result.Message =
                        ".سفارش نمیتواند تکمیل شود! زیرا حداقل یکی از محصولات دیگر در فروشگاه وجود ندارد";

                    return result;
                }
                if (item.Product.Stock < item.Quantity)
                {
                    result.StatusCode = ResultStatusCode.Conflict;
                    result.Message = ".موجودی یک یا چند محصول انتخاب شده کافی نمیباشد";

                    return result;
                }
            }
            #endregion

            var request = await _paymentGateway.RequestAsync(payment.Amount, orderId, callbackUrl);
            if (!request.IsSuccessful)
            {
                _logger.LogWarning(
                    "Payment request failed for OrderId {OrderId}: {Reason}",
                    orderId,
                    request.ErrorMessage);

                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = request.ErrorMessage;

                return result;
            }

            //add authority
            payment.Authority = request.Authority;
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Data = request.PaymentUrl + request.Authority;

            _logger.LogInformation(
                "Payment started successfully for OrderId {OrderId}", orderId);

            return result;
        }

        public async Task<ServiceResult<string>> VerifyPaymentAsync
            (string authority, string gatewayStatus)
        {
            var result = new ServiceResult<string>();

            var payment = await _unitOfWork.orderRepository.GetPaymentByAuthorityAsync(authority);

            if (payment is null)
            {
                _logger.LogWarning(
                    "Payment verification failed because the payment was not found");

                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".پرداخت یافت نشد";

                return result;
            }
          
            if (payment.Status == PaymentStatus.Completed.ToString())
            {
                result.StatusCode = ResultStatusCode.Conflict;
                result.Message = ".این پرداخت قبلاً تایید شده است";

                return result;
            }

            if (gatewayStatus != "OK")
            {
                payment.Status = PaymentStatus.Expired.ToString();
                await _unitOfWork.SaveChangesAsync();

                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".پرداخت لغو یا ناموفق بود";

                return result;
            }

            var verifyResult = await _paymentGateway.VerifyAsync(payment.Amount, authority);
            
            if (!verifyResult.IsSuccessful)
            {
                _logger.LogWarning(
                    "Payment verification failed for OrderId {OrderId}: {Reason}",
                    payment.OrderId,
                    verifyResult.ErrorMessage);

                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".تایید پرداخت انجام نشد";

                return result;
            }

            #region Reduction Of Product Stock
            var orderItems = await _unitOfWork.orderRepository
                .GetOrderItemsIncludeProductsAsync(payment.OrderId);
            
            foreach (var item in orderItems)
            {
                if (item.Product is null)
                    throw new InvalidOperationException(".محصول سفارش یافت نشد"); 

                var oldStock = item.Product.Stock;
                item.Product.Stock -= item.Quantity;

                _logger.LogInformation(
                    "Stock reduced for ProductId {ProductId}: {OldStock} -> {NewStock} after payment for OrderId {OrderId}",
                    item.Product.Id,
                    oldStock,
                    item.Product.Stock,
                    payment.OrderId);
            }
            #endregion

            //change order and payment status
            payment.Status = PaymentStatus.Completed.ToString();
            payment.Order?.Status = OrderStatus.Paid;
            
            payment.ReferenceId = verifyResult.ReferenceId;
            await _unitOfWork.SaveChangesAsync();


            result.StatusCode = ResultStatusCode.Success;
            result.Message = ".پرداخت با موفقیت تایید شد";
            result.Data = verifyResult.ReferenceId;

            _logger.LogInformation(
                "Payment verified successfully for OrderId {OrderId} with ReferenceId {ReferenceId}",
                payment.OrderId,
                verifyResult.ReferenceId);

            return result;
        }
        #endregion

        #region Update Payment
        public async Task<ServiceResult<PaymentDto>> UpdatePaymentStatusAsync
            (ChangePaymentStatusDto changePaymentStatus)
        {
            var result = new ServiceResult<PaymentDto>();

            var order = await _unitOfWork.orderRepository.GetOrderByIdAsync(changePaymentStatus.OrderId);
            if (order is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".سفارش یافت نشد";

                return result;
            }

            if (order.Payment is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".پرداخت یافت نشد";

                return result;
            }

            //get new status
            var status = Enum.Parse<PaymentStatus>(changePaymentStatus.Status, true);

            //change order status
            if (status is PaymentStatus.Expired)
            {
                if (order.Payment.Status == PaymentStatus.Completed.ToString())
                {
                    #region Return Stock To Product
                    var orderItems = await _unitOfWork.orderRepository.GetOrderItemsIncludeProductsAsync(order.Id);

                    foreach (var item in orderItems)
                    {
                        if (item.Product is null)
                            throw new InvalidOperationException(".محصول سفارش یافت نشد");

                        var oldStock = item.Product.Stock;
                        item.Product.Stock += item.Quantity;

                        _logger.LogInformation(
                            "Stock restored for ProductId {ProductId}: {OldStock} -> {NewStock} after payment expiration for OrderId {OrderId}",
                            item.Product.Id,
                            oldStock,
                            item.Product.Stock,
                            order.Id);
                    }
                    #endregion
                }
                order.Status = OrderStatus.Cancelled;
            }
            else if (status is PaymentStatus.Waiting && order.Payment.Status == PaymentStatus.Completed.ToString())
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".نمیتوان وضعیت سفارش را از وضعیت تکمیل شده به در انتظار پرداخت تغییر داد";

                return result;
            }
            else if (status is PaymentStatus.Completed && order.Payment.Status == PaymentStatus.Waiting.ToString())
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".نمیتوان وضعیت سفارش را از انتظار پرداخت وضعیت به در تکمیل شده تغییر داد";

                return result;
            }
            else if (status is PaymentStatus.Waiting && order.Payment.Status == PaymentStatus.Expired.ToString())
            {
                order.Status = OrderStatus.AwaitingPayment;
            } 
            else
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = "Waiting, Expired :مقدار ارسالی باید شامل یکی از این دو مورد باشد";

                return result;
            } 
            
            //change payment status and savechanges
            order.Payment.Status = status.ToString();
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<PaymentDto>(order.Payment);

            return result;
        }
        #endregion

        #region Update Order
        public async Task<ServiceResult<AdminOrderDto>> UpdateOrderStatusAsync(ChangeOrderStatusDto changeOrderStatus)
        {
            var result = new ServiceResult<AdminOrderDto>();

            var order = await _unitOfWork.orderRepository.GetOrderByIdAsync(changeOrderStatus.OrderId);
            if(order is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".شناسه سفارش یافت نشد";

                return result;
            }

            var oldOrderStatus = order.Status;

            //change status type string to enum
            var status = Enum.Parse<OrderStatus>(changeOrderStatus.Status, true);

            if (order.Status is OrderStatus.AwaitingPayment &&
                 !(status is OrderStatus.Cancelled || status is OrderStatus.Paid))
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = 
                    ".نمیتوانید وضعیت سفارش را از در انتظار پرداخت به چیزی به غیر از لغو شده یا پرداخت شده تغییر دهید";

                return result;
            } 
            if(order.Status is OrderStatus.AwaitingPayment && status is OrderStatus.Paid)
            {
                #region Reduction Of Product Stock
                var orderItems = await _unitOfWork.orderRepository.GetOrderItemsIncludeProductsAsync(order.Id);

                foreach(var item in orderItems)
                {
                    if(item.Product is null)
                        throw new InvalidOperationException(".محصول سفارش یافت نشد");

                    if (item.Product.IsDeleted is true)
                    { 
                        result.StatusCode = ResultStatusCode.NotFound;
                        result.Message = 
                            ".سفارش نمیتواند تکمیل شود زیرا حداقل یکی از محصولا دیگر در فروشگاه وجود ندارد";

                        return result;
                    }
                    if (item.Product.Stock < item.Quantity)
                    {
                        result.StatusCode = ResultStatusCode.BadRequest;
                        result.Message = ".موجودی محصول کافی نیست";
                     
                        return result;
                    }

                    //reduce stock
                    item.Product?.Stock -= item.Quantity;
                }
                #endregion

                order.Payment?.Status = PaymentStatus.Completed.ToString();
            }
            else if (status is OrderStatus.Cancelled)
            {
                if(order.Payment?.Status == PaymentStatus.Completed.ToString())
                {
                    #region Return Stock To Product
                    var orderItems = await _unitOfWork.orderRepository.GetOrderItemsIncludeProductsAsync(order.Id);
                
                    foreach(var item in orderItems)
                    {
                        if (item.Product is null)
                            throw new InvalidOperationException(".محصول سفارش یافت نشد");
                        
                        var oldStock = item.Product.Stock;
                        item.Product.Stock += item.Quantity;

                        _logger.LogInformation(
                            "Stock restored for ProductId {ProductId}: {OldStock} -> {NewStock} after order cancellation for OrderId {OrderId}",
                            item.Product.Id,
                            oldStock,
                            item.Product.Stock,
                            order.Id);
                    }
                    #endregion
                }

                order.Payment?.Status = PaymentStatus.Expired.ToString();                
            }
            else if(status is OrderStatus.AwaitingPayment)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = 
                    "Processing, Shipped, Delivered, Cancelled :مقدار ارسالی باید شامل یکی از این موارد باشد";

                return result;
            }

            //change order status and savechanges
            order.Status = status;
            _logger.LogInformation(
                "Order status changed for OrderId {OrderId}: {OldStatus} -> {NewStatus}",
                order.Id,
                oldOrderStatus,
                order.Status);
            await _unitOfWork.SaveChangesAsync();


            result.StatusCode = ResultStatusCode.Success;
            result.Data = await _unitOfWork.orderRepository.GetOrderForAdminAsync(changeOrderStatus.OrderId);

            return result;
        }
        #endregion
    }
}
