using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();

        Task UpdateStockAfterSale(int productId, int units);
    }
}
