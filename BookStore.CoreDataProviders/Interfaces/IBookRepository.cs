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
        public Task<int> CreateBookAsync(Book book);
        public Task<Book> GetBookByIdAsync(int id);
        public Task<IReadOnlyList<Book>> GetBooksAsync();
        public Task UpdateBookAsync(int id, Book book);
        public Task<bool> DeleteBookAsync(int id);
    }
}
