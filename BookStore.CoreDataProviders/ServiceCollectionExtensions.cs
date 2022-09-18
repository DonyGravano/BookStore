using System.Data;
using BookStore.CoreDataProviders.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.CoreDataProviders;

public static class ServiceCollectionExtensions
{
    public static void AddCoreDataProviders(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<IConnectionStringConfig, ConnectionStringConfig>();
        serviceCollection.AddTransient<IDbConnection>(sp =>
        {
            var connectionStringConfig = sp.GetRequiredService<IConnectionStringConfig>();
            return new SqliteConnection(connectionStringConfig.ConnectionString);
        });
        serviceCollection.AddTransient<IQueryExecutor, QueryExecutor>();
        serviceCollection.AddTransient<IBookRepository, SqlLiteBookRepository>();
    }
}