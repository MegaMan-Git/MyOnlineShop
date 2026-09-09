using Application.Dtos.Product;
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
            CreateMap<IEnumerable<Product>,IEnumerable<ProductDto>>();
            CreateMap<ProductDto,Product>();
            CreateMap<ProductDto,AddProductDto>();
            CreateMap<ProductDto,UpdateProductDto>();
        }
    }
}
