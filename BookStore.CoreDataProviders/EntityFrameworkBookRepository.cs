using BookStore.CoreDataProviders.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.CoreDataProviders
{
    // This should go into it's own project
    public class EntityFrameworkBookRepository : IBookRepository
    {
        private readonly BookContext _bookContext;

        public EntityFrameworkBookRepository(BookContext bookContext)
        {
            _bookContext = bookContext ?? throw new ArgumentNullException(nameof(bookContext));
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            var entry = await _bookContext.Books.AddAsync(book);

            await _bookContext.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Book> GetBookByIdAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Book>> GetBooksAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            throw new NotImplementedException();
        }
    }
}
