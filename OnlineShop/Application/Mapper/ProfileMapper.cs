using Application.Dtos.Category;
using Application.Dtos.Product;
using Application.Entities;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.AutoMapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            //product
            CreateMap<Product,ProductDto>();
            CreateMap<ProductDto,Product>();
            CreateMap<AddProductDto,Product>();
            CreateMap<UpdateProductDto,Product>();

            //category
            CreateMap<Category,CategoryDto>()
                .ForMember(cd => cd.CategoryName,c => c.MapFrom(src => src.Title));
            
            CreateMap<CategoryDto,Category>()
                .ForMember(c => c.Title, cd => cd.MapFrom(src => src.CategoryName));

            CreateMap<AddCategoryDto,Category>()
                .ForMember(c => c.Title, acd => acd.MapFrom(src => src.CategoryName));

            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(c => c.Title, ucd => ucd.MapFrom(src => src.NewCategoryName));
        }
    }
}
