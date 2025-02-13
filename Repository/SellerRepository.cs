﻿using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Services;
using MySql.Data.MySqlClient;

namespace MVCproyect.Repository
{
    public class SellerRepository : ISellerRepository
    {
        private MySqlService _context;
        public SellerRepository(MySqlService context) 
        {
            _context = context;
        }

        public async Task<List<Seller>> GetSellersAsync() 
        {
            List<Seller> sellers = new List<Seller>();

            try
            {
                using (MySqlConnection connection = _context.CreateConnection())
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM tb_seller";

                    using MySqlCommand command = new MySqlCommand(query, connection);

                    using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        sellers.Add(new Seller
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Identity = reader.GetString("Identity"),
                            CreatedOn = reader.GetDateTime("CreatedOn")
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sellers;
        }

        public async Task<Seller> GetSellerByIdAsync(int id)
        {
            Seller seller = new Seller();

            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT Id, Name, Identity, CreatedOn FROM tb_seller WHERE Id=@id";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    seller = new Seller
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Identity = reader.GetString("Identity"),
                        CreatedOn = reader.GetDateTime("CreatedOn")
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return seller;
        }

        public async Task AddSellerAsync(Seller newSeller)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "INSERT INTO tb_seller (Name, Identity, CreatedOn) VALUES (@name, @identity, @createdOn)";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@name", newSeller.Name);
                command.Parameters.AddWithValue("@identity", newSeller.Identity);
                command.Parameters.AddWithValue("@createdOn", newSeller.CreatedOn);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task DeleteSellerAsync(int id)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "DELETE FROM tb_seller WHERE Id=@id";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateSellerAsync(Seller updatedSeller)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "UPDATE tb_seller SET Name = @Name, Identity = @Identity WHERE Id = @Id";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("Id", updatedSeller.Id);
                command.Parameters.AddWithValue("Name", updatedSeller.Name);
                command.Parameters.AddWithValue("@Identity", updatedSeller.Identity);

                await command.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        public async Task<List<SellerReport>> GetSellersReport() 
        {
            List<SellerReport> report = new();

            try 
            { 
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT \r\n    " +
                    "s.SellerId AS Seller_Id, \r\n    " +
                    "s.SellerName AS Seller_Name, \r\n    " +
                    "(SELECT COUNT(*) FROM sales WHERE SellerId = s.SellerId) AS Total_Sales,\r\n    " +
                    "COUNT(sd.Units) AS Total_Units,\r\n    " +
                    "(SELECT SUM(sales.TotalSale) FROM sales WHERE SellerId = s.SellerId) AS Total_Income,\r\n" +
                    "SUM(s.TotalSale) AS Total_Income \r\n" +
                    "FROM sale_details sd\r\n" +
                    "JOIN sales s ON s.Id = sd.SaleId\r\n" +
                    "GROUP BY s.SellerId;";

                MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    report.Add(new SellerReport
                    {
                        SellerId = reader.GetInt32("Seller_Id"),
                        SellerName = reader.GetString("Seller_Name"),
                        TotalSales = reader.GetInt32("Total_Sales"),
                        TotalUnits = reader.GetInt32("Total_Units"),
                        TotalIncome = reader.GetDecimal("Total_Income")
                    });
                }
            }
            catch (Exception ex)
            {
                  Console.WriteLine(ex.Message); 
            }

            return report;
        }
    }
}
