using Azure;
using Azure.Core;
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
            await _productRepository.CreateAsync(product);
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync(
            new string[]
            {
                nameof(Product.Translations),
                nameof(Product.CreatedBy)
            });

            return products.Adapt<List<ProductResponse>>();
        }

        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _productRepository.GetOne(filter, new string[]
            {
                nameof(Product.Translations),
                nameof(Product.CreatedBy)
            });

            if(product == null) return null;
            return product.Adapt<ProductResponse>();
        } 

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOne(c => c.Id == id);
            if(product == null) return false;

            _fileService.Delete(product.MainImage);
            return await _productRepository.DeleteAsync(product);
        }
    }
}
