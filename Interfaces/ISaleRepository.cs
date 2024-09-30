using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Product>> GetSalesAsync();

        Task<Product> GetSaleByIdAsync(int id);

        Task AddSaleAsync(Product newProduct);

        Task UpdateSaleAsync(Product newProduct);

        Task DeleteSaleAsync(int id);
    }
}
