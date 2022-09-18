using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using BookStore.CoreDataProviders.Interfaces;
using BookStore.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;

namespace BookStore.Application.UnitTests;

[TestFixture]
public class BookServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _mockBookRepository = new Mock<IBookRepository>();
    }

    private Mock<IBookRepository> _mockBookRepository;

    private BookService Sut => new(_mockBookRepository.Object);

    // This test checks the constructors parameters all have guard clauses
    [Test]
    public void Ctor_NullArgument_ThrowsNullArgumentException()
    {
        var fixture = new Fixture();
        var assertion = new GuardClauseAssertion(fixture.Customize(new AutoMoqCustomization()));
        assertion.Verify(typeof(BookService).GetConstructors());
    }

    [Test]
    [AutoData]
    public async Task CreateBookAsync_CallsRespository_WithParameter(Book book)
    {
        await Sut.CreateBookAsync(book);

        var parameters = new List<Book>();
        _mockBookRepository.Verify(br => br.CreateBookAsync(Capture.In(parameters)), Times.Once);

        using (new AssertionScope())
        {
            parameters.Should().HaveCount(1);
            parameters.Single().Should().Be(book);
        }
    }

    [Test]
    [AutoData]
    public async Task DeleteBookAsync_CallsRespository_WithParameter(int id)
    {
        await Sut.DeleteBookAsync(id);

        var parameters = new List<int>();
        _mockBookRepository.Verify(br => br.DeleteBookAsync(Capture.In(parameters)), Times.Once);

        using (new AssertionScope())
        {
            parameters.Should().HaveCount(1);
            parameters.Single().Should().Be(id);
        }
    }

    [Test]
    [AutoData]
    public async Task GetBookByIdAsync_CallsRespository_WithParameter(int id)
    {
        await Sut.GetBookByIdAsync(id);

        var parameters = new List<int>();
        _mockBookRepository.Verify(br => br.GetBookByIdAsync(Capture.In(parameters)), Times.Once);

        using (new AssertionScope())
        {
            parameters.Should().HaveCount(1);
            parameters.Single().Should().Be(id);
        }
    }

    [Test]
    [AutoData]
    public async Task UpdateBookAsync_CallsRespository_WithParameter(int id, Book book)
    {
        await Sut.UpdateBookAsync(id, book);

        var idParams = new List<int>();
        var bookParams = new List<Book>();
        _mockBookRepository.Verify(br => br.UpdateBookAsync(Capture.In(idParams), Capture.In(bookParams)), Times.Once);

        using (new AssertionScope())
        {
            idParams.Should().HaveCount(1);
            idParams.Single().Should().Be(id);
            bookParams.Should().HaveCount(1);
            bookParams.Single().Should().Be(book);
        }
    }

    [Test]
    public async Task GetBooksAsync_CallsRespository()
    {
        await Sut.GetBooksAsync();

        _mockBookRepository.Verify(br => br.GetBooksAsync(), Times.Once);
    }
}