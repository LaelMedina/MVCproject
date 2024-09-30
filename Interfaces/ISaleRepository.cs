using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Sale>> GetSalesAsync();

        Task<Sale> GetSaleByIdAsync(int id);

        Task AddSaleAsync(Sale newSale);

        Task UpdateSaleAsync(Sale newSale);

        Task DeleteSaleAsync(int id);
    }
}
