using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Controllers;
using Deposit.Models;

namespace Deposit.Services
{
    public class ProductServices
    {
        public List<Product> GetAllProducts(IRepository<Product> repository)
        {
            return repository.ReadAll();
        }

        public Product GetProduct(IRepository<Product> repository, Guid id)
        {
            return repository.Read(id);
        }

        public Product CreateProduct(IRepository<Product> repository, ProductDto productDto)
        {
            var dimensions = Dimensions.MakeDimensions(productDto.Dimensions.Width, productDto.Dimensions.Height, productDto.Dimensions.Depth);
            var product = Product.MakeProduct(productDto.Name, productDto.Description, productDto.Price, dimensions);
            repository.Add(product);
            return product;
        }

        public void DeleteProduct(IRepository<Product> repository, Guid id)
        {
            repository.Delete(id);
        }

        public void UpdateProduct(IRepository<Product> repository, Guid id, ProductDto productDto)
        {
            var product = repository.Read(id);
            
            if (product == null)
                throw new ArgumentException("Product not found.");
            
            if (productDto.Name != string.Empty || productDto.Description != string.Empty)
                product.Rename(productDto.Name, productDto.Description);
            
            if (productDto.Price != 0)
                product.UpdatePrice(productDto.Price);
            
            if (productDto.Dimensions != null)
                product.Redimension(productDto.Dimensions.Width, productDto.Dimensions.Height, productDto.Dimensions.Depth);
            
            repository.Update(id, product);
        }
    }
}