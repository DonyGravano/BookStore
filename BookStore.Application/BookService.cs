using BookStore.Application.Interfaces;
using BookStore.CoreDataProviders.Interfaces;
using BookStore.Models;

namespace BookStore.Application;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
    }

    public async Task<int> CreateBookAsync(Book book)
    {
        return await _bookRepository.CreateBookAsync(book);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        return await _bookRepository.DeleteBookAsync(id);
    }

    public async Task<Book> GetBookByIdAsync(int id)
    {
        return await _bookRepository.GetBookByIdAsync(id);
    }

    public async Task<IReadOnlyList<Book>> GetBooksAsync()
    {
        return await _bookRepository.GetBooksAsync();
    }

    public async Task UpdateBookAsync(int id, Book book)
    {
        await _bookRepository.UpdateBookAsync(id, book);
    }
}