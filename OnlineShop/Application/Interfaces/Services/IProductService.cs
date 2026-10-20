using Application.Dtos.Product;
using Application.Filters;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync
            (QueryStringParameters parameters);
        Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id); 

        Task<ServiceResult<ProductDto>> AddProductAsync(AddProductDto productDto);
        
        Task<ServiceResult<ProductDto>> UpdateProductAsync(UpdateProductDto productDto);
        
        Task<ServiceResult<string>> SoftDeleteProductAsync(int id);
    }
}
