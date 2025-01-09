using AutoMapper;
using FreshCart.Business.DTOs;
using FreshCart.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore());

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.TotalAmount,
                    opt => opt.MapFrom(src => src.Items.Sum(i =>
                        (i.Product.DiscountPercentage.HasValue
                            ? i.Product.Price * (1 - i.Product.DiscountPercentage.Value / 100)
                            : i.Product.Price) * i.Quantity)));
            CreateMap<CartDto, Cart>();

            CreateMap<CartItem, CartItemDto>();
            CreateMap<CartItemDto, CartItem>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.DiscountPercentage,
                    opt => opt.MapFrom(src => src.Product.DiscountPercentage))
                .ForMember(dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.UnitPrice *
                        (1 - (src.DiscountPercentage ?? 0) / 100) * src.Quantity));
            CreateMap<OrderItemDto, OrderItem>();

            // Map Review
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<ReviewAddDto, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // User Mapping
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
