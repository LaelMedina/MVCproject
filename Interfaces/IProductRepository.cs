﻿using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task AddProductAsync(Product newProduct);
        
        Task UpdateProductAsync(Product newProduct);

        Task DeleteProductAsync(int id);
    }
}
