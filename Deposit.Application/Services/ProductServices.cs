using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Application.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.Application.Dtos;

namespace Deposit.Application.Services
{
    public class ProductServices
    {
        private readonly IRepository<Product> _repository;

        public ProductServices(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public List<ProductView> GetAllProducts()
        {
            var products = _repository.ListAll();

            return products.Where(p => !p.IsDeleted).
                Select(i => new ProductView()
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    Description = i.Description,
                    Name = i.Name,
                    Price = i.Price,
                    Sku = i.Sku
                })
                .ToList();
        }

        public ProductView GetProduct(Guid id)
        {
            var product = _repository.ListAll().FirstOrDefault(p => p.Id == id);

            if (product == null)
                return null;

            if (product.IsDeleted)
                throw new InvalidOperationException("Product deleted.");

            return new ProductView()
            {
                Id = product.Id,
                Amount = product.Amount,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                Sku = product.Sku
            };
        }

        public ProductView CreateProduct(ProductDto productDto)
        {
            var dimensions = Dimensions.MakeDimensions(productDto.Dimensions.Width, productDto.Dimensions.Height, productDto.Dimensions.Depth);
            var product = Product.MakeProduct(productDto.Name, productDto.Description, productDto.Price, dimensions);
            _repository.Add(product);
            return new ProductView()
            {
                Id = product.Id,
                Amount = product.Amount,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                Sku = product.Sku
            };
        }

        public void DeleteProduct(Guid id)
        {
            _repository.Delete(id);
        }

        public void UpdateProduct(Guid id, ProductDto productDto)
        {
            var product = _repository.ListAll().FirstOrDefault(p => p.Id == id);
            
            if (product == null)
                throw new ArgumentException("Product not found.");
            
            if (productDto.Name != string.Empty || productDto.Description != string.Empty)
                product.Rename(productDto.Name, productDto.Description);
            
            if (productDto.Price != 0)
                product.UpdatePrice(productDto.Price);
            
            if (productDto.Dimensions != null)
                product.Redimension(productDto.Dimensions.Width, productDto.Dimensions.Height, productDto.Dimensions.Depth);
            
            _repository.Update(product);
        }
    }
}