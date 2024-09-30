using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Services;
using MySql.Data.MySqlClient;

public class ProductRepository : IProductRepository
{
    private readonly MySqlService _context;

    public ProductRepository(MySqlService context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProductsAsync()
    {

        var products = new List<Product>();

        try
        {
            using (MySqlConnection connection = _context.CreateConnection())
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM products";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Description = reader.GetString("description"),
                        Price = reader.GetDecimal("price"),
                        Stock = reader.GetInt32("stock"),
                        CreatedAt = reader.GetDateTime("CreatedAt")
                    });
                }
            }

        }
        catch (Exception ex)
        { 
            Console.WriteLine(ex.Message);
        }

        return products;
    }


    public Task<Product> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddProductAsync(Product newProduct)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProductAsync(Product newProduct)
    {
        throw new NotImplementedException();
    }
}
