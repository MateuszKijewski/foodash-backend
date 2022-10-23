using FooDash.Application.Products.Dtos.Basic;
using FooDash.Domain.Entities.Products;
using Mapster;

namespace FooDash.Application.MapperConfigurations
{
    public static class ProductMapperConfiguration
    {
        public static TypeAdapterConfig AddProductConfiguration(this TypeAdapterConfig config)
        {
            config
                .NewConfig<MenuDto, Menu>();
            config
                .NewConfig<Menu, ReadMenuDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<MenuCategoryProductDto, MenuCategoryProduct>();
            config
                .NewConfig<MenuCategoryProduct, ReadMenuCategoryProductDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<ProductDto, Product>();
            config
                .NewConfig<Product, ReadProductDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<ProductAttribute, ReadProductAttributeDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<ProductAttributeOptionSetValueDto, ProductAttributeOptionSetValue>();
            config
                .NewConfig<ProductAttributeOptionSetValue, ReadProductAttributeOptionSetValueDto>()
                .IgnoreNullValues(true);

            config
                .NewConfig<ProductCategoryDto, ProductCategory>();
            config
                .NewConfig<ProductCategory, ReadProductCategoryDto>()
                .IgnoreNullValues(true);

            return config;
        }
    }
}