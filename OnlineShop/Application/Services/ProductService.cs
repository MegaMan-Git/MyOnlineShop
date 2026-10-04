using Application.AutoMapper;
using Application.Dtos.Product;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        #region DI
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Get Product
        public async Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var result = new ServiceResult<IEnumerable<ProductDto>>();

            //get all products
            var products = await _unitOfWork.productRepository.GetProductsAsync();
            
            if(!products.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "هیچ محصولی یافت نشد.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<IEnumerable<ProductDto>>(products);

            return result;
        }
        
        public async Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var result = new ServiceResult<ProductDto>();
            
            //try get product
            var product = await _unitOfWork.productRepository.GetProductByIdAsync(id);

            if(product is null)
            {
               result.StatusCode = ResultStatusCode.NotFound;
               result.Message = "محصولی با آیدی مورد نظر یافت نشد.";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<ProductDto>(product);

            return result;
        }
        #endregion

        #region Add Product
        public async Task<ServiceResult<ProductDto>> AddProductAsync(AddProductDto productDto)
        {
            var result = new ServiceResult<ProductDto>();

            // is productname before exist?
            var isDuplicate = await _unitOfWork.productRepository
                .IsProductNameDuplicateAsync(productDto.ProductName);
            if (isDuplicate)
            {
                result.StatusCode = ResultStatusCode.Conflict;
                result.Message = "محصول دیگری با این نام قبلا ثبت شده است.";

                return result;
            }
            
            //map
            var product = _mapper.Map<Product>(productDto);
            
            //add and savechanges
            await _unitOfWork.productRepository.AddProductAsync(product);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Message = "محصول اضافه شد.";
            result.Data = _mapper.Map<ProductDto>(product);

            return result;
        }
        #endregion

        #region Update Product
        public async Task<ServiceResult<ProductDto>> UpdateProductAsync(UpdateProductDto productDto)
        {
            var result = new ServiceResult<ProductDto>();

            // is productname before exist?
            var isDuplicate = await _unitOfWork.productRepository
                .IsProductNameDuplicateAsync(productDto.ProductName,productDto.Id);
            if (isDuplicate)
            {
                result.StatusCode = ResultStatusCode.Conflict;
                result.Message = "محصول دیگری با این نام قبلا ثبت شده است.";

                return result;
            }

            //map
            var product = _mapper.Map<Product>(productDto);

            //add and savechanges
            await _unitOfWork.productRepository.UpdateProductAsync(product);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Message = "محصول بروزرسانی شد.";
            result.Data = _mapper.Map<ProductDto>(product);

            return result;
        }
        #endregion

        #region Delete Product
        public async Task<ServiceResult<string>> DeleteProductAsync(int id)
        {
            var result = new ServiceResult<string>();

            var product = await _unitOfWork.productRepository.GetProductByIdAsync(id);
            if(product is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = "محصول یافت نشد.";

                return result;
            }
            await _unitOfWork.productRepository.DeleteProductAsync(product);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;

            return result;
        }
        #endregion
    }
}
