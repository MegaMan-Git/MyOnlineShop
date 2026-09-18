using Application.Dtos.Cart.Cartitem;
using Application.Dtos.Category;
using Application.Dtos.Payment;
using Application.Dtos.Product;
using Application.Entities;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
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
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<AddProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            //category
            CreateMap<Category, CategoryDto>()
                .ForMember(cd => cd.CategoryName, c => c.MapFrom(src => src.Title));

            CreateMap<CategoryDto, Category>()
                .ForMember(c => c.Title, cd => cd.MapFrom(src => src.CategoryName));

            CreateMap<AddCategoryDto, Category>()
                .ForMember(c => c.Title, acd => acd.MapFrom(src => src.CategoryName));

            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(c => c.Title, ucd => ucd.MapFrom(src => src.NewCategoryName));

            //order
            CreateMap<Payment, PaymentDto>()
                .ForMember(pd => pd.Status,p => p.MapFrom(src => Enum.Parse<PaymentStatus>(src.Status)));
        }
    }
}
