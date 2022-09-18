using AutoFixture.NUnit3;
using BookStore.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.CoreDataProviders.UnitTests
{
    [TestFixture]
    public class EntityFrameworkBookRepositoryUnitTests
    {
        private Mock<BookContext> _mockDbContext;
        private BookContext _testDbContext;

        [SetUp]
        public void SetUp()
        {
            _mockDbContext = new Mock<BookContext>();
            _testDbContext = new BookContext();
        }

        private EntityFrameworkBookRepository Sut => new(_testDbContext);

        [Test]
        [AutoData]
        public async Task CreateBook_AddsNewBook(Book book)
        {
            await Sut.CreateBookAsync(book);

            _testDbContext.Books.Should().HaveCount(1);
        }

        [Test]
        public async Task CreateBook_ReturnsNewBook()
        {
            Assert.True(false);
        }

        [Test]
        public async Task DeleteBook_BookExists_DeletesBook()
        {
            Assert.True(false);
        }

        [Test]
        public async Task DeleteBook_BookDoesNotExist_ThrowsInvalidOperationException()
        {
            Assert.True(false);
        }

        [Test]
        public async Task GetBookById_BookExists_ReturnsCorrectBook()
        {
            Assert.True(false);
        }

        [Test]
        public async Task GetBookById_BookDoesNotExist_ThrowsInvalidOperationException()
        {
            Assert.True(false);
        }

        [Test]
        public async Task GetBooks_ReturnsBooks()
        {
            Assert.True(false);
        }

        [Test]
        public async Task UpdateBook_BookExists_ReturnsUpdatedBook()
        {
            Assert.True(false);
        }

        [Test]
        public async Task UpdateBook_BookDoesNotExist_ThrowsInvalidOperationException()
        {
            Assert.True(false);
        }
    }
}
