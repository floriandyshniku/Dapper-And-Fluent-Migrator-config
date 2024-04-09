using Dapper;
using MinimalApiTest.Model;
using MinimalApiTest.Service;

namespace MinimalApiTest.Endpoints
{
    public static class CustomerEndpoints
    {

        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder)
        {

            var group = builder.MapGroup("customer");

            group.MapGet("getCustomers", async (SqlConnectionFactory sqlConnectionFactory) =>
            {

                using var connection = sqlConnectionFactory.Create();

                const string sql = "SELECT * FROM Customer";

                var customers = await connection.QueryAsync<Customer>(sql);
                return Results.Ok(customers);
            });
        }

    }
}
