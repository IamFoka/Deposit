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
        public List<ProductView> GetAllProducts(IRepository<Product> repository)
        {
            var products = repository.ListAll();

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

        public ProductView GetProduct(IRepository<Product> repository, Guid id)
        {
            var product = repository.ListAll().FirstOrDefault(p => p.Id == id);

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

        public ProductView CreateProduct(IRepository<Product> repository, ProductDto productDto)
        {
            var dimensions = Dimensions.MakeDimensions(productDto.Dimensions.Width, productDto.Dimensions.Height, productDto.Dimensions.Depth);
            var product = Product.MakeProduct(productDto.Name, productDto.Description, productDto.Price, dimensions);
            repository.Add(product);
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

        public void DeleteProduct(IRepository<Product> repository, Guid id)
        {
            repository.Delete(id);
        }

        public void UpdateProduct(IRepository<Product> repository, Guid id, ProductDto productDto)
        {
            var product = repository.ListAll().FirstOrDefault(p => p.Id == id);
            
            if (product == null)
                throw new ArgumentException("Product not found.");
            
            if (productDto.Name != string.Empty || productDto.Description != string.Empty)
                product.Rename(productDto.Name, productDto.Description);
            
            if (productDto.Price != 0)
                product.UpdatePrice(productDto.Price);
            
            if (productDto.Dimensions != null)
                product.Redimension(productDto.Dimensions.Width, productDto.Dimensions.Height, productDto.Dimensions.Depth);
            
            repository.Update(product);
        }
    }
}