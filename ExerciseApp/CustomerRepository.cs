using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ExerciseApp
{
    public class CustomerRepository
    {
        public Customer GetById(int id)
        {
            Customer customer = null;
            var connectionString = ConfigurationManager.ConnectionStrings["appDatabase"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetCustomerById"
                };

                var paramete = new SqlParameter("@customerId", SqlDbType.Int) { Value = id };
                command.Parameters.Add(paramete);
                
                connection.Open();
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    customer = new Customer
                    {
                        Id = int.Parse(reader["CustomerId"].ToString()),
                        Name = reader["Name"].ToString(),
                        CustomerStatus = (CustomerStatus) int.Parse(reader["CustomerStatus"].ToString())
                    };
                }
            }

            return customer;
        }
    }
}