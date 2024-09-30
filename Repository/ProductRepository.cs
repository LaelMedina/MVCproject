using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Services;
using MySql.Data.MySqlClient;

public class ProductRepository : IProductRepository
{
    private readonly MySqlService _context;
    private readonly IdGeneratorService _idGeneratorService;

    public ProductRepository(MySqlService context, IdGeneratorService idGeneratorService)
    {
        _context = context;
        _idGeneratorService = idGeneratorService;
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

    public async Task<Product> GetProductByIdAsync(int id)
    {

        Product product = new Product();

        try
        {
            using MySqlConnection connection = _context.CreateConnection();

            await connection.OpenAsync();

            string query = "SELECT * FROM products WHERE id=@id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                product = new Product()
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Description = reader.GetString("description"),
                    Price = reader.GetDecimal("price"),
                    Stock = reader.GetInt32("stock"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return product;
    }

    public async Task AddProductAsync(Product newProduct)
    {

        try
        {
            using MySqlConnection connection = _context.CreateConnection();

            await connection.OpenAsync();

            string query = "INSERT INTO products (id, name, description, price, stock) VALUES (@id, @name, @description, @price, @stock)";

            using MySqlCommand command = new MySqlCommand(query, connection);

            newProduct.Id = await _idGeneratorService.GenerateNextIdAsync("products");

            command.Parameters.AddWithValue("@id", newProduct.Id);
            command.Parameters.AddWithValue("@name", newProduct.Name);
            command.Parameters.AddWithValue("@description", newProduct.Description);
            command.Parameters.AddWithValue("@price", newProduct.Price);
            command.Parameters.AddWithValue("@stock", newProduct.Stock);

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }

    public async Task DeleteProductAsync(int id)
    {
        try
        {
            using MySqlConnection connection = _context.CreateConnection();

            await connection.OpenAsync();

            string query = "DELETE FROM products WHERE id=@id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task UpdateProductAsync(Product updatedProduct)
    {
        try
        {
            using MySqlConnection connection = _context.CreateConnection();

            await connection.OpenAsync();

            string query = "UPDATE products SET name = @name, description = @description, price = @price, stock = @stock WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", updatedProduct.Id);
            command.Parameters.AddWithValue("@name", updatedProduct.Name);
            command.Parameters.AddWithValue("@description", updatedProduct.Description);
            command.Parameters.AddWithValue("@price", updatedProduct.Price);
            command.Parameters.AddWithValue("@stock", updatedProduct.Stock);

            await command.ExecuteNonQueryAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }

    }
}
