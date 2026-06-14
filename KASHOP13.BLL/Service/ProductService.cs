using Azure;
using Azure.Core;
using KASHOP13.BLL.Extentions;
using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Migrations;
using KASHOP13.DAL.Models;
using KASHOP13.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IProductRepository productRepository, IFileService fileService, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();

            if(request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }

            if(request.SubImages != null)
            {
                foreach(var image in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    
                    product.Images.Add(new ProductImage
                    {

                        ImagePath = imagePath
                    });
                }
            }

            await _productRepository.CreateAsync(product);
        }

        public async Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(PaginationRequest request)
        {
            var query = _productRepository.GetQuerable(
                p => p.Status == EntityStatus.Active
                , 
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                    nameof(Product.Images)
                });

            var paginated = await query.ToPaginationAsync(request.Page , request.Limit );

            return new PaginationResponse<ProductResponse>
            {
                TotalCount = paginated.TotalCount,
                Page = paginated.Page,
                Limit = paginated.Limit,
                Data = paginated.Data.Adapt<List<ProductResponse>>(),
                
            };
        }

        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _productRepository.GetOne(filter, new string[]
            {
                nameof(Product.Translations),
                nameof(Product.CreatedBy),
                nameof(Product.Images)
            });

            if(product == null) return null;
            return product.Adapt<ProductResponse>();
        } 

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOne(c => c.Id == id,
                new string[]
                {
                    nameof(Product.Images)
                });

            if(product == null) return false;

            _fileService.Delete(product.MainImage);

            foreach(var image in product.Images)
            {
                _fileService.Delete(image.ImagePath);
            }
            return await _productRepository.DeleteAsync(product);
        }

        public async Task<bool> UpdateProduct(int id, ProductUpdateRequest request)
        {
            var product = await _productRepository.GetOne(p => p.Id == id,
                 new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.Images)
                });

            var oldImage = product.MainImage;

            request.Adapt(product);

            if(request.Translations != null)
            {
                foreach(var translationRequest in request.Translations)
                {
                    var existing = product.Translations.FirstOrDefault(t => t.Language == translationRequest.Language);
                    if(existing != null)
                    {
                        if(translationRequest.Name != null)
                        {
                            existing.Name = translationRequest.Name;
                        }
                        if (translationRequest.Description != null)
                        {
                            existing.Description = translationRequest.Description;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if(request.MainImage != null)
            {
                _fileService.Delete(oldImage);
                product.MainImage = await _fileService.UploadAsync(request.MainImage);
            }
            else
            {
                product.MainImage = oldImage;
            }

            if (request.SubImages != null)
            {
                foreach(var image in product.Images)
                {
                    _fileService.Delete(image.ImagePath);
                }
                product.Images.Clear();

                foreach (var image in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    product.Images.Add(new ProductImage { ImagePath = imagePath });
                }

            }

            if(request.NewImages != null)
            {
                foreach (var image in request.NewImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    product.Images.Add(new ProductImage { ImagePath = imagePath });
                }
            }

            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> ToggleStatus(int id)
        {
            var product = await _productRepository.GetOne(p => p.Id == id);
            if (product is null) return false;

            product.Status = product.Status == EntityStatus.Active ? EntityStatus.InActive : EntityStatus.Active;

            return await _productRepository.UpdateAsync(product);
        }
    }
}
