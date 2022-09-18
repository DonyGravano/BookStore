using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.CoreDataProviders.Interfaces
{
    public interface IBookRepository
    {
        public Task<Book> CreateBook(Book book);
        public Task<Book> GetBookById(Book book);
        public Task<IReadOnlyList<Book>> GetBooks(Book book);
        public Task<Book> UpdateBook(int id, Book book);
        public Task<bool> DeleteBook(int id);
    }
}
