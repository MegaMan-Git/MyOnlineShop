using Application.Dtos.Product;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id);
        Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<ServiceResult<ProductDto>> AddProductAsync(AddProductDto productDto);
        Task<ServiceResult<ProductDto>> UpdateProductAsync(UpdateProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
