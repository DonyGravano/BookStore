using BookStore.Models;

namespace BookStore.Application.Interfaces
{
    public interface IBookService
    {
        public Task<Book> CreateBook(Book book);
        public Task<Book> GetBookById(Book book);
        public Task<IReadOnlyList<Book>> GetBooks(Book book);
        public Task<Book> UpdateBook(int id, Book book);
        public Task<bool> DeleteBook(int id);
    }
}
