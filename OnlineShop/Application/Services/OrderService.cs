using Application.Dtos.Cart.Cartitem;
using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Dtos.Payment;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        #region DI
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Get Order&OrderItem
        public async Task<ServiceResult<IEnumerable<AdminOrderDto>>> GetAllOrdersForAdminAsync()
        {
            var result = new ServiceResult<IEnumerable<AdminOrderDto>>();

            var orders = await _unitOfWork.orderRepository.GetAllOrdersForAdminAsync();
            if(!orders.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "هیچ سفارشی در حال حاضر ثبت نشده.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = orders;

            return result;
        }

        public async Task<ServiceResult<IEnumerable<AdminOrderItemDto>>> GetAllOrderItemsForAdminAsync()
        {
            var result = new ServiceResult<IEnumerable<AdminOrderItemDto>>();
            
            var orderItems = await _unitOfWork.orderRepository.GetAllOrderItemsForAdminAsync();
            if(!orderItems.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "هیچ سفارشی در حال حاضر ثبت نشده.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = orderItems;

            return result;
        }

        public async Task<ServiceResult<CustomerOrderItemDto>> GetOrderItemAsync
            (string userId,int orderItemId, int orderId)
        {
            var result = new ServiceResult<CustomerOrderItemDto>();

            //validate request
            var order = await _unitOfWork.orderRepository.GetOrderAsync(orderId, userId);
            if(order is null)
            {
                result.StatusCode = ResultStatusCode.Forbidden;
                result.Message = "سفارش یافت نشد یا دسترسی به آن امکان پذیر نیست.";

                return result;
            }

            var orderItemDto = await _unitOfWork.orderRepository.GetOrderItemAsync(orderItemId, order.Id);
            if(orderItemDto is null)
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
            (string userId,int orderId)
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

            var orderItemsDto = await _unitOfWork.orderRepository.GetOrderItemsAsync(order.Id);
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
        public async Task<ServiceResult<PaymentDto>> GetPaymentAsync(string userId,int orderId)
        {
            var result = new ServiceResult<PaymentDto>();

            //validate request
            var order = await _unitOfWork.orderRepository.GetOrderAsync(orderId, userId);
            if (order is null)
            {
                result.StatusCode = ResultStatusCode.Forbidden;
                result.Message = "سفارش یافت نشد یا دسترسی به آن امکان پذیر نیست.";

                return result;
            }

            var payment = await _unitOfWork.orderRepository.GetPaymentAsync(order.Id);
            if (payment is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "پرداختی برای این سفارش ثبت نشده.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<PaymentDto>(payment);

            return result;
        }

        public async Task<ServiceResult<IEnumerable<PaymentDto>>> GetPaymentsAsync(string userId)
        {
            var result = new ServiceResult<IEnumerable<PaymentDto>>();

            var payments = await _unitOfWork.orderRepository.GetPaymentsAsync(userId);
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
            (string userId,int cartItemId)
        {
            var result = new ServiceResult<CustomerOrderItemDto>();

            //get cart and caritem
            var cart = await _unitOfWork.cartRepository.GetCartAsync(userId);
            if(cart is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "سبد خرید یافت نشد.";

                return result;
            }
            var cartItem = await _unitOfWork.cartRepository.GetCartItemAsync(cartItemId,cart.Id);
            if(cartItem is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "آیتمی با شناسه ارسالی برای شما یافت نشد.";

                return result;
            }


            //check product stock
            var product = await _unitOfWork.productRepository.GetProductByIdAsync(cartItem.ProductId);
            if(product is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "محصول از قبل انتخاب شده دیگر در فروشگاه وجود ندارد.";

                return result;
            }
            if(product.Stock < cartItem.Quantity)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = "تعداد سفارش درخواستی بیشتر از موجودی فعلی انبار میباشد! لطفا سفارش خود را اصلاح کنید";

                return result;
            }
            //change product stock
            product.Stock -= cartItem.Quantity;
            await _unitOfWork.productRepository.UpdateProductAsync(product);

            
            //add new order record in database
            var newOrder = new Order { UserId = userId };
            await _unitOfWork.orderRepository.AddOrderAsync(newOrder);
            await _unitOfWork.SaveChangesAsync();
            
            //add orderitem and payment
            var orderItem = new OrderItem
            {
                OrderId = newOrder.Id,
                ProductId = product.Id,
                Quantity = cartItem.Quantity,
                UnitPrice = product.Price
            };
            await _unitOfWork.orderRepository.AddOrderItemAsync(orderItem);
            await _unitOfWork.orderRepository.AddPaymentAsync(newOrder.Id, product.Price * cartItem.Quantity);

            
            //delete cartitem
            await _unitOfWork.cartRepository.DeleteCartItemAsync(cartItem);
            

            //savechanges
            await _unitOfWork.SaveChangesAsync();


            //return status
            result.StatusCode = ResultStatusCode.Success;
            result.Data = await _unitOfWork.orderRepository.GetOrderItemAsync(orderItem.Id,newOrder.Id);

            return result;
        }

        public async Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> CreateOrdersFromCartAsync
            (string userId)
        {
            var result = new ServiceResult<IEnumerable<CustomerOrderItemDto>>();
            List<Product> products = new();
            int totalPrice = 0;

            //get cart and cartitem dtos
            var cart = await _unitOfWork.cartRepository.GetCartAsync(userId);
            if (cart is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "سبد خرید یافت نشد.";

                return result;
            }
            var cartItemDtos = await _unitOfWork.cartRepository.GetCustomerCartItemsAsync(cart.Id);
            if (!cartItemDtos.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "هیچ محصولی در سبد خرید شما وجود ندارد.";

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
                        $"محصول از قبل انتخاب شده {item.ProductName} دیگر در فروشگاه وجود ندارد.");
                    continue;
                }

                products.Add(product);

                if (product.Stock < item.Quantity)
                {
                    result.StatusCode = ResultStatusCode.BadRequest;
                    result.Errors.Add(
                        $"تعداد سفارش درخواستی محصول {item.ProductName} بیشتر از موجودی انبار است");
                }
            }
            //if there are problems exist
            if (result.StatusCode == ResultStatusCode.BadRequest)
            {
                result.Message = "مشکلی در هنگام ثبت سفارش پیش آمده";

                return result;
            }

            
            //add new order record in database
            var newOrder = new Order { UserId = userId };
            await _unitOfWork.orderRepository.AddOrderAsync(newOrder);
            await _unitOfWork.SaveChangesAsync();


            var cartItemListDtos = cartItemDtos.ToList();
            for (int i = 0; i < cartItemListDtos.Count;i++)
            {
                var item = cartItemListDtos[i];
                var product = products[i];

                //change product stock
                product.Stock -= item.Quantity;
                await _unitOfWork.productRepository.UpdateProductAsync(product);

                //add orderitem
                await _unitOfWork.orderRepository.AddOrderItemAsync(new OrderItem
                {
                    OrderId = newOrder.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product!.Price
                });
                
                totalPrice += product.Price * item.Quantity;
            }
            //add payment
            await _unitOfWork.orderRepository.AddPaymentAsync(newOrder.Id, totalPrice);
            
            //delete cartitems
            var cartItems = await _unitOfWork.cartRepository.GetCartItemsAsync(cart.Id);
            foreach (var cartItem in cartItems)
            {
                await _unitOfWork.cartRepository.DeleteCartItemAsync(cartItem);
            }

            //savechanges
            await _unitOfWork.SaveChangesAsync();

            //return status
            result.StatusCode = ResultStatusCode.Success;
            result.Data = await _unitOfWork.orderRepository.GetOrderItemsAsync(newOrder.Id);

            return result;
        }
        #endregion

        #region Update Payment
        public async Task<ServiceResult<PaymentDto>> UpdatePaymentStatusAsync
            (AdminChangeStatusDto adminChangeStatus)
        {
            var result = new ServiceResult<PaymentDto>();

            var payment = await _unitOfWork.orderRepository.GetPaymentAsync(adminChangeStatus.OrderId);
            if(payment is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "شناسه ارسالی یافت نشد.";

                return result;
            }
            //change status
            payment.Status = adminChangeStatus.Status.ToString();

            await _unitOfWork.orderRepository.UpdatePaymentStatusAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<PaymentDto>(payment);

            return result;
        }
        #endregion
    }
}
