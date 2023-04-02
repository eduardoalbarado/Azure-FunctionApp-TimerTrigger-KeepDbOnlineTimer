using System;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace KeepDbOnlineTimer;
    public class Function1
{
    [FunctionName("Function1")]
    public void Run([TimerTrigger("0 */30 * * * *"
#if DEBUG
    ,RunOnStartup= true
#endif
    )]TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        string connectionString = Environment.GetEnvironmentVariable("MyConnectionString");
        string queryString = "SELECT [Id],[Name] FROM [dbo].[Table]";
        string message = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                message = $"Query OK";
                log.LogInformation(message);
            }
            reader.Close();
        };
    }
}