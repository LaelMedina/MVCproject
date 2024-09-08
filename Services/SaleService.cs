using Microsoft.AspNetCore.Mvc;
using MVCproyect.Models;
using MySql.Data.MySqlClient;

namespace MVCproyect.Services
{
    public class SaleService
    {

        private readonly MySqlConnection _connection;

        private readonly List<PaymentMethod> _paymentMethods;

        public SaleService(MySqlService connection) 
        { 
            _connection = connection.CreateConnection();
            _paymentMethods = new List<PaymentMethod>();
        }

        public List<PaymentMethod> GetPaymentMethods()
        {
            try 
            {

                _connection.Open();

                string query = "SELECT * FROM paymentmethods";

                using MySqlCommand command = new MySqlCommand(query, _connection);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
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
