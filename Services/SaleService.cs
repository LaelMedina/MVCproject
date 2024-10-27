using Microsoft.AspNetCore.Mvc;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MySql.Data.MySqlClient;

namespace MVCproyect.Services
{
    public class SaleService : ISaleService
    {

        private readonly MySqlService _context;

        public SaleService(MySqlService context) 
        { 
            _context = context;
        }

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            List<Currency> _currencies = new List<Currency>();

            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT * FROM tb_currency";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Currency currency = new Currency()
                    {
                        Id = reader.GetInt32("id"),
                        CurrencyName = reader.GetString("currency"),
                        Acronym = reader.GetString("acronym"),
                    };

                    _currencies.Add(currency);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return _currencies;
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsAsync()
        {
            List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();

            try 
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT * FROM paymentmethods";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    PaymentMethod paymentMethod = new PaymentMethod() 
                    { 
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                    };

                    _paymentMethods.Add(paymentMethod);
                }

            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }
            
            return _paymentMethods;
        }

    }
}
