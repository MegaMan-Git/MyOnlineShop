using Application.Dtos.Category;
using Application.Entities;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Application.Filters;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        #region DI
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Get Category
        public async Task<ServiceResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync
            (QueryStringParameters parameters)
        {
            var result = new ServiceResult<IEnumerable<CategoryDto>>();

            var categories = await _unitOfWork.categoryRepository.GetAllCategoriesAsync(parameters);
            if(!categories.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".هیچ دسته بندی در حال حاضر ثبت نشده";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return result;
        }

        public async Task<ServiceResult<CategoryDto>> GetCategoryByIdAsync(int categoryId)
        {
            var result = new ServiceResult<CategoryDto>();
            
            var category = await _unitOfWork.categoryRepository.GetCategoryByIdAsync(categoryId);
            if(category is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".دسته بندی مورد نظر یافت نشد";

                return result;
            }

            result.StatusCode= ResultStatusCode.Success;
            result.Data = _mapper.Map<CategoryDto>(category);

            return result;
        }
        #endregion

        #region Add Category
        public async Task<ServiceResult<CategoryDto>> AddCategoryAsync(AddCategoryDto categoryDto)
        {
            var result = new ServiceResult<CategoryDto>();

            var isDuplicated = await _unitOfWork.categoryRepository
                .IsDuplicateCategoryNameAsync(categoryDto.CategoryName);
            if (isDuplicated)
            {
                result.StatusCode = ResultStatusCode.Conflict;
                result.Message = ".این دسته بندی از قبل وجود دارد";

                return result;
            }

            //map
            var category = _mapper.Map<Category>(categoryDto);

            //add and savechanges
            await _unitOfWork.categoryRepository.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<CategoryDto>(category);

            return result;
        }
        #endregion

        #region Update Category
        public async Task<ServiceResult<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto categoryDto)
        {
            var result = new ServiceResult<CategoryDto>();

            //is category exist?
            var category = await _unitOfWork.categoryRepository.GetCategoryByIdAsync(categoryDto.Id);
            if(category is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".دسته بندی مورد نظر یافت نشد";

                return result;
            }

            //is default category?
            if(category.Id == -1)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".نمیتوان نام دسته بندی پیشفرض را عوض کرد";

                return result;
            }
            
            //is category duplicate
            var isDuplicated = await _unitOfWork.categoryRepository
                .IsDuplicateCategoryNameAsync(categoryDto.NewCategoryName, categoryDto.Id);
            if (isDuplicated)
            {
                result.StatusCode = ResultStatusCode.Conflict;
                result.Message = ".نام دسته بندی تکراری است";

                return result;
            }

            //map
            _mapper.Map(categoryDto, category);

            //add and savechanges
            await _unitOfWork.categoryRepository.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;
            result.Data = _mapper.Map<CategoryDto>(category);

            return result;
        }
        #endregion

        #region Delete Category
        public async Task<ServiceResult<string>> SoftDeleteCategoryAsync(int categoryId)
        {
            var result = new ServiceResult<string>();

            var category = await _unitOfWork.categoryRepository.GetCategoryByIdAsync(categoryId);
            if(category is null)
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".دسته بندی یافت نشد";

                return result;
            }

            //you can not delete default category!
            if(category.Id == -1)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = ".امکان حذف برای این دسته بندی وجود ندارد";

                return result;
            }

            //change category product to default category after delete category
            var products = await _unitOfWork.productRepository.GetProductsByCategoryId(categoryId);            
            foreach(var product in products)
            {
                product.CategoryId = -1;
            }

            //Delete
            category.IsDeleted = true;
            await _unitOfWork.categoryRepository.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();

            result.StatusCode = ResultStatusCode.Success;

            return result;
        }
        #endregion
    }
}
