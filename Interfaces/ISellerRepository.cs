using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface ISellerRepository
    {
        Task<List<Seller>> GetSellersAsync();

        Task<Seller> GetSellerByIdAsync(int id);

        Task AddSellerAsync(Seller newSeller);

        Task UpdateSellerAsync(Seller newSeller);

        Task DeleteSellerAsync(int id);
    }
}
