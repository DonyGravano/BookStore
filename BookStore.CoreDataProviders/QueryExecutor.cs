using System.Data;
using Dapper;

namespace BookStore.CoreDataProviders;

public class QueryExecutor : IQueryExecutor
{
    private readonly IDbConnection _dbConnection;

    public QueryExecutor(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
    }

    public async Task<IReadOnlyList<TResult>> QueryAsync<TResult>(string query, object? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(query)) throw new ArgumentNullException(nameof(query));

        return (await _dbConnection.QueryAsync<TResult>(query, parameters)).ToList();
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));

        return await _dbConnection.ExecuteAsync(sql, parameters);
    }
}