using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace KASHOP13.BLL.Mapping
{
    public static class MapsterConfig
    {
        
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.Category_Id, source => source.Id)
                .Map(dest => dest.User, source => source.CreatedBy.UserName)
                .Map(dest => dest.Name, source => source.Translations.Where(
                    t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()
                );

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.UserCreated, source => source.CreatedBy.UserName)
                .Map(dest => dest.Name, source => source.Translations.Where(
                    t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()
                )
                .Map(dest => dest.MainImage, source => $"{HttpContextHelper.Accessor.HttpContext.Request.Scheme}://{HttpContextHelper.Accessor.HttpContext.Request.Host}/images/{source.MainImage}")
                .Map(dest => dest.SubImages, source => source.Images.Select(
                    img => $"{HttpContextHelper.Accessor.HttpContext.Request.Scheme}://{HttpContextHelper.Accessor.HttpContext.Request.Host}/images/{img.ImagePath}"));
            
            //TypeAdapterConfig<ProductRequest, Product>.NewConfig()
            //    .Map(dest => dest.SubImages, source => $"{HttpContextHelper.Accessor.HttpContext.Request.Scheme}://{HttpContextHelper.Accessor.HttpContext.Request.Host}/images/{source.SubImages}");


            TypeAdapterConfig<ProductUpdateRequest, Product>.NewConfig()
                .IgnoreNullValues(true);

            TypeAdapterConfig<Brand, BrandResponse>.NewConfig()
                .Map(dest => dest.BrandId, source => source.Id)
                .Map(dest => dest.User, source => source.CreatedBy.UserName)
                .Map(dest => dest.Name, source => source.Translations.Where(
                    t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()
                )
                .Map(dest => dest.Logo, source => $"{HttpContextHelper.Accessor.HttpContext.Request.Scheme}://{HttpContextHelper.Accessor.HttpContext.Request.Host}/images/{source.Logo}");

            TypeAdapterConfig<Cart, CartResponse>.NewConfig()
                .Map(dest => dest.ProductName, source => source.Product.Translations.Where(
                    t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()
                )
                .Map(dest => dest.Price, source => source.Product.Price)
                .Map(dest => dest.ProductImage, source => $"{HttpContextHelper.Accessor.HttpContext.Request.Scheme}://{HttpContextHelper.Accessor.HttpContext.Request.Host}/images/{source.Product.MainImage}"); 
        }
    }
}
