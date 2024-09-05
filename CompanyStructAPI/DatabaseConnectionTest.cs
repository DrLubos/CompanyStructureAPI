using Microsoft.Data.SqlClient;
namespace CompanyStructAPI
{
    public class ConnectionTest
    {
        private readonly IConfiguration _configuration;

        public ConnectionTest(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void TestConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to the database was successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
