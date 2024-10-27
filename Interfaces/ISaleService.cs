using MVCproyect.Models;

namespace MVCproyect.Interfaces
{
    public interface ISaleService
    {
        Task<List<PaymentMethod>> GetPaymentMethodsAsync();

        Task<List<Currency>> GetCurrenciesAsync();
    }
}
