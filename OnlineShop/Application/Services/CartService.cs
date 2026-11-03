using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Application.Filters;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    /// <summary>
    /// منطق مشاهده و مدیریت سبدهای خرید و آیتم های آن ها را اجرا میکند
    /// </summary>
    public class CartService : ICartService
    {
        #region DI
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// وابستگی مورد نیاز سرویس سبد خرید را دریافت میکند
        /// </summary>
        /// <param name="unitOfWork">ها و ذخیره تغییرات Repository برای دسترسی به</param>
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Get Cart&CartItems
        /// <summary>
        /// فهرست سبدهای خرید را برای مدیر دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست سبدهای خرید یا پیام خطا</returns>
        public async Task<ServiceResult<IEnumerable<AdminCartDto>>> GetAllCartsForAdminAsync
            (QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<AdminCartDto>>();

            var carts = await _unitOfWork.cartRepository.GetAllCartsForAdminAsync(parameters);
            if (!carts.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".در حال حاضر هیچ سبد خریدی توسط مشتریان ثبت نشده";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = carts;

            return result;
        }

        /// <summary>
        /// فهرست آیتم های سبدهای خرید را برای مدیر دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست آیتم ها یا پیام خطا</returns>
        public async Task<ServiceResult<IEnumerable<AdminCartItemDto>>> GetAllCartItemsForAdminAsync
            (QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<AdminCartItemDto>>();

            var cartItems = await _unitOfWork.cartRepository.GetAllCartItemsForAdminAsync(parameters);
            if (!cartItems.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".در حال حاضر هیچ سبد خریدی توسط مشتریان ثبت نشده";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = cartItems;

            return result;
        }

        /// <summary>
        /// آیتم های سبد خرید مشتری را دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>نتیجه شامل آیتم های سبد خرید یا پیام خطا</returns>
        public async Task<ServiceResult<IEnumerable<CustomerCartItemDto>>> GetCustomerCartItemsAsync
            (string userId)
        {
            var result = new ServiceResult<IEnumerable<CustomerCartItemDto>>();

            var cart = await _unitOfWork.cartRepository.GetCartAsync(userId);
            if (cart is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".سبد خرید در حال حاضر خالی است";

                return result;
            }

            var cartItems = await _unitOfWork.cartRepository.GetCustomerCartItemsAsync(cart.Id);
            if (!cartItems.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".سبد خرید در حال حاضر خالی است";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = cartItems;

            return result;
        }

        /// <summary>
        /// یک آیتم مشخص از سبد خرید مشتری را دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemId">شناسه آیتم سبد خرید</param>
        /// <returns>نتیجه شامل آیتم سبد خرید یا پیام خطا</returns>
        public async Task<ServiceResult<CustomerCartItemDto>> GetCustomerCartItemAsync
            (string userId, int cartItemId)
        {
            var result = new ServiceResult<CustomerCartItemDto>();

            var cartItem = await _unitOfWork.cartRepository.GetCustomerCartItemDtoAsync(userId, cartItemId);
            if (cartItem is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".محصولی با این مشخصات در سبد خرید یافت نشد";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = cartItem;

            return result;
        }
        #endregion

        #region Add Cart&CartItem
        /// <summary>
        /// محصولی را به سبد خرید مشتری اضافه میکند یا مقدار آیتم موجود را افزایش میدهد
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemDto">محصول و تعداد موردنظر</param>
        /// <returns>نتیجه شامل آیتم ایجادشده یا به روزشده</returns>
        public async Task<ServiceResult<CustomerCartItemDto>> AddCartItemAsync
            (string userId, AddCartItemDto cartItemDto)
        {
            var result = new ServiceResult<CustomerCartItemDto>();

            //does the requested product already exist?
            var product = await _unitOfWork.productRepository
                .GetProductByIdAsync(cartItemDto.ProductId);
            
            if (product is null)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".محصولی با آیدی خواسته شده وجود ندارد";

                return result;
            }

            //does the user already have a shopping cart?
            var userCart = await _unitOfWork.cartRepository.GetCartAsync(userId);

            //if shopping cart doesn't exist then create new shopping cart
            if (userCart is null)
            {
                userCart = new Cart { UserId = userId };
                await _unitOfWork.cartRepository.AddCartAsync(userCart);
                await _unitOfWork.SaveChangesAsync();
            }

            //is the product already in the shopping cart?
            var existingCartItem = await _unitOfWork.cartRepository
                .GetCartItemByProductIdAsync(userCart.Id, product.Id);
            if (existingCartItem is not null)
            {
                //does the requested quantity of the product exceed the available stock?
                if (product.Stock < (cartItemDto.Quantity + existingCartItem.Quantity))
                {
                    result.StatusCode = ResultStatusCode.BadRequest;
                    result.Message = ".موجودی محصول درخواست شده کافی نمیباشد";

                    return result;
                }

                existingCartItem.Quantity += cartItemDto.Quantity;

                await _unitOfWork.cartRepository.UpdateCartItemAsync(existingCartItem);
                await _unitOfWork.SaveChangesAsync();

                result.StatusCode = ResultStatusCode.Success;
                result.Data = await _unitOfWork.cartRepository
                    .GetCustomerCartItemDtoAsync(userId, existingCartItem.Id);

                return result;
            }

            //does the requested quantity of the product exceed the available stock?
            if (product.Stock < cartItemDto.Quantity)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".موجودی محصول درخواست شده کافی نمیباشد";

                return result;
            }

            //add new cartitem
            var cartItem = new CartItem
            {
                CartId = userCart.Id,
                ProductId = product.Id,
                Quantity = cartItemDto.Quantity,
            };
            await _unitOfWork.cartRepository.AddCartItemAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Data = await _unitOfWork.cartRepository
                .GetCustomerCartItemDtoAsync(userId, cartItem.Id);

            return result;

        }
        #endregion

        #region Update CartItem
        /// <summary>
        /// تعداد یک آیتم موجود در سبد خرید را تغییر میدهد
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemDto">شناسه آیتم و تعداد جدید</param>
        /// <returns>نتیجه شامل آیتم به روزشده یا پیام خطا</returns>
        public async Task<ServiceResult<CustomerCartItemDto>> UpdateCartItemAsync
            (string userId, UpdateCartItemDto cartItemDto)
        {
            var result = new ServiceResult<CustomerCartItemDto>();

            //does the user already have a shopping cart?
            var userCart = await _unitOfWork.cartRepository.GetCartAsync(userId);

            //if shopping cart doesn't exist then create new shopping cart
            if (userCart is null)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = "در خواست نامعتبر است، زیرا سبد خرید خالی است.";

                return result;
            }

            //does the requested cart item already exist?
            var cartItem = await _unitOfWork.cartRepository
                .GetCartItemAsync(cartItemDto.CartItemId, userCart.Id);
            
            if (cartItem is null)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = "آیتمی با آیدی ارسال شده در سبد خرید وجود ندارد.";

                return result;
            }

            //does the requested quantity of the product exceed the available stock?
            var product = await _unitOfWork.productRepository.GetProductByIdAsync(cartItem.ProductId);
            if (product is null)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message =
                    ".محصول مربوط به این آیتم از سایت حذف شده، بنابراین از سبد خرید شما به طور خودکار حذف شد";
                await _unitOfWork.cartRepository.DeleteCartItemAsync(cartItem);
                await _unitOfWork.SaveChangesAsync();

                return result;
            }
            if (product.Stock < cartItemDto.Quantity)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".موجودی محصول درخواست شده کافی نمیباشد";

                return result;
            }

            //change cart item quantity
            cartItem.Quantity = cartItemDto.Quantity;

            await _unitOfWork.cartRepository.UpdateCartItemAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Data = await _unitOfWork.cartRepository.GetCustomerCartItemDtoAsync(userId, cartItem.Id);

            return result;
        }
        #endregion

        #region Delete CartItem
        /// <summary>
        /// یک آیتم را از سبد خرید مشتری حذف میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemId">شناسه آیتم سبد خرید</param>
        /// <returns>نتیجه حذف آیتم</returns>
        public async Task<ServiceResult<string>> DeleteCartItemAsync(string userId, int cartItemId)
        {
            var result = new ServiceResult<string>();

            //find cart
            var cart = await _unitOfWork.cartRepository.GetCartAsync(userId);
            if (cart is null)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".سبد خریدی هنوز ساخته نشده";

                return result;
            }

            //find cartitem
            var cartItem = await _unitOfWork.cartRepository.GetCartItemAsync(cartItemId, cart.Id);
            if (cartItem is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".محصولی با این مشخصات در سبد خرید یافت نشد";

                return result;
            }

            ///delete cartitem
            await _unitOfWork.cartRepository.DeleteCartItemAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();


            result.StatusCode = ResultStatusCode.Success;

            return result;
        }

        /// <summary>
        /// تمام آیتم های سبد خرید مشتری را حذف میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>نتیجه حذف آیتم ها</returns>
        public async Task<ServiceResult<string>> ClearCartItemsAsync(string userId)
        {
            var result = new ServiceResult<string>();

            //find cart
            var cart = await _unitOfWork.cartRepository.GetCartAsync(userId);
            if (cart is null)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".سبد خریدی هنوز ساخته نشده";

                return result;
            }

            //delete cartitems
            var cartItems = await _unitOfWork.cartRepository.GetCartItemsAsync(cart.Id);
            await _unitOfWork.cartRepository.DeleteCartItemsAsync(cartItems);


            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;

            return result;
        }
        #endregion
    }
}
