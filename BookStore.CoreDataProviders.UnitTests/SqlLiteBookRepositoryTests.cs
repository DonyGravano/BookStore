using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using BookStore.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;

namespace BookStore.CoreDataProviders.UnitTests
{
    [TestFixture]
    public class SqlLiteBookRepositoryTests
    {
        private Mock<IQueryExecutor> _mockQueryExecutor;

        [SetUp]
        public void SetUp()
        {
            _mockQueryExecutor = new Mock<IQueryExecutor>();
        }

        private SqlLiteBookRepository Sut => new(_mockQueryExecutor.Object);

        // This test checks the constructors parameters all have guard clauses
        [Test]
        public void Ctor_NullArgument_ThrowsNullArgumentException()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture.Customize(new AutoMoqCustomization()));
            assertion.Verify(typeof(SqlLiteBookRepository).GetConstructors());
        }

        [Test]
        [AutoData]
        public async Task CreateBook_CallsQueryExecutorWithInsertString(Book book, int newId)
        {
            _mockQueryExecutor.Setup(qe => qe.QueryAsync<int>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(new List<int> { newId });

            await Sut.CreateBookAsync(book);

            _mockQueryExecutor.Verify(qe => qe.QueryAsync<int>(It.Is<string>(s => s.Contains("INSERT INTO books (Author, Title, Price) VALUES (@Author, @Title, @Price);")), book), Times.Once);
        }

        [Test]
        [AutoData]
        public async Task CreateBook_ReturnsNewId(Book book, int newId)
        {
            _mockQueryExecutor.Setup(qe => qe.QueryAsync<int>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(new List<int> { newId });

            var result = await Sut.CreateBookAsync(book);

            result.Should().Be(newId);
        }

        [Test]
        [AutoData]
        public async Task CreateBook_ReturnsMultipleIds_ThrowsInvalidOperationException(Book book, List<int> ids)
        {
            _mockQueryExecutor.Setup(qe => qe.QueryAsync<int>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(ids);

            Func<Task> act = async () => await Sut.CreateBookAsync(book);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        [AutoData]
        public async Task DeleteBook_CallsQueryExecutorWithDeleteString(int id)
        {
            _mockQueryExecutor.Setup(qe => qe.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(1);

            await Sut.DeleteBookAsync(id);

            var parameters = new List<object>();

            _mockQueryExecutor.Verify(qe => qe.ExecuteAsync(It.Is<string>(s => s.Contains("DELETE FROM books WHERE Id = @id")), Capture.In(parameters)), Times.Once);

            using (new AssertionScope())
            {
                parameters.Should().HaveCount(1);
                parameters.Single().Should().BeEquivalentTo(new { id });
            }
        }

        [Test]
        [AutoData]
        public async Task DeleteBook_BookDoesNotExist_ThrowsInvalidOperationException(int id)
        {
            _mockQueryExecutor.Setup(qe => qe.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(0);

            Func<Task> act = async () => await Sut.DeleteBookAsync(id);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"No rows were deleted with id: {id}");
        }

        [Test]
        [AutoData]
        public async Task DeleteBook_TooManyBooksMatchDelete_ThrowsException(int id)
        {
            _mockQueryExecutor.Setup(qe => qe.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(2);

            Func<Task> act = async () => await Sut.DeleteBookAsync(id);

            await act.Should().ThrowAsync<Exception>().WithMessage($"Too many records were deleted for id: {id}");
        }

        [Test]
        [AutoData]
        public async Task GetBookById_CallsQueryExecutorWithCorrectQuery(int id, Book book)
        {
            _mockQueryExecutor.Setup(qe => qe.QueryAsync<Book>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(new List<Book> { book });

            await Sut.GetBookByIdAsync(id);

            var parameters = new List<object>();

            _mockQueryExecutor.Verify(qe => qe.QueryAsync<Book>(It.Is<string>(s => s.Contains("SELECT * FROM books WHERE id = @id")), Capture.In(parameters)), Times.Once);

            using (new AssertionScope())
            {
                parameters.Should().HaveCount(1);
                parameters.Single().Should().BeEquivalentTo(new { id });
            }
        }

        [Test]
        [AutoData]
        public async Task GetBookById_BookDoesNotExist_ThrowsArgumentException(int id)
        {
            _mockQueryExecutor.Setup(qe => qe.QueryAsync<Book>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(new List<Book> { });

            Func<Task> act = async () => await Sut.GetBookByIdAsync(id);

            await act.Should().ThrowAsync<ArgumentException>().WithMessage($"No books were found with id: {id}");
        }

        [Test]

        [AutoData]
        public async Task GetBookById_MultipleBooksReturned_ThrowsArgumentException(int id, List<Book> books)
        {
            _mockQueryExecutor.Setup(qe => qe.QueryAsync<Book>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(books);

            Func<Task> act = async () => await Sut.GetBookByIdAsync(id);

            await act.Should().ThrowAsync<ArgumentException>().WithMessage($"Too many books were found with id: {id}");
        }

        [Test]
        [AutoData]
        public async Task GetBooks_CallsQueryExecutorWithCorrectQuery_ReturnsBooks(List<Book> books)
        {
            _mockQueryExecutor.Setup(qe => qe.QueryAsync<Book>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(books);

            var result = await Sut.GetBooksAsync();

            _mockQueryExecutor.Verify(qe => qe.QueryAsync<Book>(It.Is<string>(s => s.Contains("SELECT * FROM books")), null), Times.Once);
            result.Should().BeEquivalentTo(books);
        }

        [Test]
        [AutoData]
        public async Task UpdateBook_CallsQueryExecutorWithCorrectQuery(int id, Book book)
        {
            _mockQueryExecutor.Setup(qe => qe.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(1);
            

            await Sut.UpdateBookAsync(id, book);

            _mockQueryExecutor.Verify(qe => qe.ExecuteAsync(It.Is<string>(s => s.Contains($"UPDATE books SET Author = @Author, Title = @Title, Price = @Price WHERE Id = @Id")), 
                It.Is<Book>(b => b.Id == id && b.Price == book.Price && b.Author == book.Author && b.Title == book.Title)), Times.Once);            
        }

        [Test]
        [AutoData]
        public async Task UpdateBook_BookDoesNotExist_ThrowsInvalidOperationException(int id)
        {
            _mockQueryExecutor.Setup(qe => qe.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(0);

            Func<Task> act = async () => await Sut.DeleteBookAsync(id);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"No rows were deleted with id: {id}");
        }

        [Test]
        [AutoData]
        public async Task UpdateBook_TooManyBooksMatchDelete_ThrowsException(int id)
        {
            _mockQueryExecutor.Setup(qe => qe.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(2);

            Func<Task> act = async () => await Sut.DeleteBookAsync(id);

            await act.Should().ThrowAsync<Exception>().WithMessage($"Too many records were deleted for id: {id}");
        }
    }
}
