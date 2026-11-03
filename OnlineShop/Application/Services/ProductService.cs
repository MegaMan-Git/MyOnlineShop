using Application.Dtos.Product;
using Application.Filters;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    /// <summary>
    /// منطق مدیریت محصولات، موجودی و حذف نرم محصولات را اجرا میکند
    /// </summary>
    public class ProductService : IProductService
    {
        #region DI
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// وابستگی های موردنیاز سرویس محصول را دریافت میکند
        /// </summary>
        /// <param name="unitOfWork">ها و ذخیره تغییرات Repository برای دسترسی به</param>
        /// <param name="mapper">ها به یکدیگر DTO برای تبدیل موجودیت ها و</param>
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Get Product
        /// <summary>
        /// فهرست محصولات را با مرتب سازی و صفحه بندی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست محصولات یا پیام خطا</returns>
        public async Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync
            (QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<ProductDto>>();

            //get all products
            var products = await _unitOfWork.productRepository
                .GetAllProductsAsync(parameters);

            if (!products.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "هیچ محصولی یافت نشد.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<IEnumerable<ProductDto>>(products);

            return result;
        }

        /// <summary>
        /// محصولی را با شناسه آن دریافت میکند
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>نتیجه شامل محصول یا پیام خطا</returns>
        public async Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var result = new ServiceResult<ProductDto>();

            //try get product
            var product = await _unitOfWork.productRepository.GetProductByIdAsync(id);

            if (product is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".محصولی با آیدی مورد نظر یافت نشد";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<ProductDto>(product);

            return result;
        }
        #endregion

        #region Add Product
        /// <summary>
        /// محصول جدیدی را به فروشگاه اضافه میکند
        /// </summary>
        /// <param name="productDto">اطلاعات محصول جدید</param>
        /// <returns>نتیجه شامل محصول ایجادشده یا پیام خطا</returns>
        public async Task<ServiceResult<ProductDto>> AddProductAsync(AddProductDto productDto)
        {
            var result = new ServiceResult<ProductDto>();

            //is category exist?
            var category = await _unitOfWork.categoryRepository
                .GetCategoryByIdAsync(productDto.CategoryId!.Value);
            
            if (category is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".دسته بندی مورد نظر یافت نشد";

                return result;
            }

            //is productname before exist?
            var isDuplicate = await _unitOfWork.productRepository
                .IsProductNameDuplicateAsync(productDto.ProductName);
            if (isDuplicate)
            {
                result.StatusCode = ResultStatusCode.Conflict;
                result.Message = ".محصول دیگری با این نام قبلا ثبت شده است";

                return result;
            }

            //map
            var product = _mapper.Map<Product>(productDto);

            //add and savechanges
            await _unitOfWork.productRepository.AddProductAsync(product);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Message = ".محصول اضافه شد";
            result.Data = _mapper.Map<ProductDto>(product);

            return result;
        }
        #endregion

        #region Update Product
        /// <summary>
        /// اطلاعات محصول را به روزرسانی میکند
        /// </summary>
        /// <param name="productDto">اطلاعات جدید محصول</param>
        /// <returns>نتیجه شامل محصول به روزشده یا پیام خطا</returns>
        public async Task<ServiceResult<ProductDto>> UpdateProductAsync(UpdateProductDto productDto)
        {
            var result = new ServiceResult<ProductDto>();

            //is product exist?
            var product = await _unitOfWork.productRepository.GetProductByIdAsync(productDto.Id);
            if (product is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".محصول مورد نظر یافت نشد";

                return result;
            }

            //is category exist?
            var category = await _unitOfWork.categoryRepository
                .GetCategoryByIdAsync(productDto.CategoryId!.Value);

            if (category is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".دسته بندی مورد نظر یافت نشد";

                return result;
            }

            //is productname before exist?
            var isDuplicate = await _unitOfWork.productRepository
                .IsProductNameDuplicateAsync(productDto.ProductName, productDto.Id);
            if (isDuplicate)
            {
                result.StatusCode = ResultStatusCode.Conflict;
                result.Message = ".محصول دیگری با این نام قبلا ثبت شده است";

                return result;
            }

            //map
            _mapper.Map(productDto, product);

            //update and savechanges
            await _unitOfWork.productRepository.UpdateProductAsync(product);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Message = ".محصول بروزرسانی شد";
            result.Data = _mapper.Map<ProductDto>(product);

            return result;
        }
        #endregion

        #region Delete Product
        /// <summary>
        /// محصول را به صورت نرم حذف کرده و آیتم های مربوط به آن را از سبدهای خرید حذف میکند
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>نتیجه حذف محصول</returns>
        public async Task<ServiceResult<string>> SoftDeleteProductAsync(int id)
        {
            var result = new ServiceResult<string>();

            //find product
            var product = await _unitOfWork.productRepository.GetProductByIdAsync(id);

            if (product is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".محصول یافت نشد";

                return result;
            }

            //delete cartitems include product
            var cartItems = await _unitOfWork.cartRepository.GetCartItemsByProductIdAsync(product.Id);
            await _unitOfWork.cartRepository.DeleteCartItemsAsync(cartItems);


            //softdelete product and savechanges
            product.IsDeleted = true;
            await _unitOfWork.productRepository.UpdateProductAsync(product);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;

            return result;
        }
        #endregion
    }
}
