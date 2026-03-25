using AutoMapper;
using Ecommerce.Application.Dtos.Categories;
using Ecommerce.Application.Dtos.Products;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Helpers;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Products
        CreateMap<ProductCreateDto, Product>();
        CreateMap<Product, ProductDetailsDto>();

        CreateMap<ProductVariantDto, ProductVariant>();
        CreateMap<ProductVariant, ProductVariantDto>(); // <-- هنا
        CreateMap<ProductImageDto, ProductImage>();
        CreateMap<ProductImage, ProductImageDto>(); // <-- هنا
        CreateMap<ProductSpecificationDto, ProductSpecification>();
        CreateMap<ProductSpecification, ProductSpecificationDto>(); // <-- هنا

        // Categories
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
    }
}