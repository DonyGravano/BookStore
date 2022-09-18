using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.CoreDataProviders
{
    public interface IQueryExecutor
    {
        Task<IReadOnlyList<TResult>> QueryAsync<TResult>(string query, object? parameters = null);

        Task<int> ExecuteAsync(string sql, object? parameters = null);
    }
}
