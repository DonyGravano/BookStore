using BookStore.CoreDataProviders.Interfaces;
using BookStore.Models;

namespace BookStore.CoreDataProviders;

public class SqlLiteBookRepository : IBookRepository
{
    private readonly IQueryExecutor _queryExecutor;

    public SqlLiteBookRepository(IQueryExecutor queryExecutor)
    {
        _queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
    }

    public async Task<int> CreateBookAsync(Book book)
    {
        var returnedIds = await _queryExecutor.QueryAsync<int>(
            "INSERT INTO books (Author, Title, Price) VALUES (@Author, @Title, @Price); SELECT last_insert_rowid();",
            book);

        return returnedIds.Single();
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        //This could potentially do a get prior to check the the record exists and only one matches

        var affectedRows = await _queryExecutor.ExecuteAsync("DELETE FROM books WHERE Id = @id", new { id });

        if (affectedRows < 1) throw new InvalidOperationException($"No rows were deleted with id: {id}");

        if (affectedRows > 1) throw new Exception($"Too many records were deleted for id: {id}");

        return true;
    }

    public async Task<Book> GetBookByIdAsync(int id)
    {
        var returnedBooks = await _queryExecutor.QueryAsync<Book>("SELECT * FROM books WHERE id = @id", new { id });

        if (!returnedBooks.Any()) throw new ArgumentException($"No books were found with id: {id}");

        if (returnedBooks.Count > 1) throw new ArgumentException($"Too many books were found with id: {id}");

        return returnedBooks.Single();
    }

    public async Task<IReadOnlyList<Book>> GetBooksAsync()
    {
        return await _queryExecutor.QueryAsync<Book>("SELECT Id, Author, Title, CAST(Price AS DOUBLE) AS Price FROM books");
    }

    public async Task UpdateBookAsync(int id, Book book)
    {
        //This could potentially do a get prior to check the the record exists and only one matches
        book.Id = id;
        var affectedRows =
            await _queryExecutor.ExecuteAsync(
                "UPDATE books SET Author = @Author, Title = @Title, Price = @Price WHERE Id = @Id", book);

        if (affectedRows < 1) throw new InvalidOperationException($"No rows were updated with id: {id}");

        if (affectedRows > 1) throw new Exception($"Too many records were updated for id: {id}");
    }
}