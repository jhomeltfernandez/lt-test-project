using AutoMapper;
using LTTestProject.Models.DTOs;
using LTTestProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models
{
    public static class Mappings
    {
        public static MapperConfiguration CreateConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

                cfg.CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            });
        }
    }
}
