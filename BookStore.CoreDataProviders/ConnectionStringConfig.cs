using BookStore.CoreDataProviders.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BookStore.CoreDataProviders;

public class ConnectionStringConfig : IConnectionStringConfig
{
    public ConnectionStringConfig(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("BookStoreDb");
    }

    public string ConnectionString { get; set; }
}