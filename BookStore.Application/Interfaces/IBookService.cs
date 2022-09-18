using BookStore.Models;

namespace BookStore.Application.Interfaces;

public interface IBookService
{
    public Task<int> CreateBookAsync(Book book);
    public Task<Book> GetBookByIdAsync(int id);
    public Task<IReadOnlyList<Book>> GetBooksAsync();
    public Task UpdateBookAsync(int id, Book book);
    public Task<bool> DeleteBookAsync(int id);
}