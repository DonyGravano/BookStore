using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using BookStore.Api.Controllers;
using BookStore.Application.Interfaces;
using BookStore.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;

namespace BookStore.Api.UnitTests;

[TestFixture]
public class BookControllerTests
{
    [SetUp]
    public void SetUp()
    {
        _mockBookService = new Mock<IBookService>();
        _mockValidator = new Mock<IValidator<Book>>();

        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private Mock<IBookService> _mockBookService;
    private Mock<IValidator<Book>> _mockValidator;

    private BookController Sut => new(_mockBookService.Object, _mockValidator.Object);

    // This test checks the constructors parameters all have guard clauses
    [Test]
    public void Ctor_NullArgument_ThrowsNullArgumentException()
    {
        var fixture = new Fixture();
        var assertion = new GuardClauseAssertion(fixture.Customize(new AutoMoqCustomization()));
        assertion.Verify(typeof(BookController).GetConstructors());
    }

    [Test]
    public void Methods_AllHaveRouteAttribute()
    {
        typeof(BookController).Methods().Should().BeDecoratedWith<RouteAttribute>();
    }

    [Test]
    public void Methods_AllHaveAtLeastOneProducesResponseCodeAttribute()
    {
        typeof(BookController).Methods().Should().BeDecoratedWith<ProducesResponseTypeAttribute>();
    }

    [Test]
    public void Methods_AllHaveHttpMethod()
    {
        typeof(BookController).Methods().Should().BeDecoratedWith<HttpMethodAttribute>();
    }

    [Test]
    [AutoData]
    public async Task BooksAsync_CallsService_ReturnsBooks(List<Book> books)
    {
        _mockBookService.Setup(bs => bs.GetBooksAsync()).ReturnsAsync(books);

        var response = await Sut.BooksAsync();
        _mockBookService.Verify(bs => bs.GetBooksAsync(), Times.Once);
        response.Result.Should().BeOfType<OkObjectResult>();
        var returnedBooks = response.Result as OkObjectResult;
        returnedBooks.Value.Should().BeEquivalentTo(books.OrderByDescending(b => b.Title));
    }

    [Test]
    [AutoData]
    public async Task PostBook_ValidModel_CallsServiceAndReturnsNewId(Book book, int id)
    {
        _mockBookService.Setup(bs => bs.CreateBookAsync(book)).ReturnsAsync(id);

        var response = await Sut.PostBook(book);

        _mockBookService.Verify(bs => bs.CreateBookAsync(book), Times.Once);
        response.Result.Should()
            .Match<ObjectResult>(r => r.StatusCode == StatusCodes.Status201Created && r.Value.As<int>() == id);
    }

    [Test]
    [AutoData]
    public async Task PostBook_InvalidModel_ReturnsValidationErrors(Book book, int id,
        ValidationResult validationResult)
    {
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _mockBookService.Setup(bs => bs.CreateBookAsync(book)).ReturnsAsync(id);

        var response = await Sut.PostBook(book);
        var validationMessage = string.Join('|', validationResult.Errors.Select(e => e.ErrorMessage));
        _mockBookService.Verify(bs => bs.CreateBookAsync(It.IsAny<Book>()), Times.Never);
        response.Result.Should().Match<BadRequestObjectResult>(r =>
            r.StatusCode == StatusCodes.Status400BadRequest && r.Value.As<string>() == validationMessage);
    }

    [Test]
    [AutoData]
    public async Task PostBook_ServiceThrowsException_PropagatesException(Book book, int id, Exception exception)
    {
        _mockBookService.Setup(bs => bs.CreateBookAsync(It.IsAny<Book>())).ThrowsAsync(exception);

        Func<Task> act = async () => await Sut.PostBook(book);

        await act.Should().ThrowAsync<Exception>().WithMessage(exception.Message);
        _mockBookService.Verify(bs => bs.CreateBookAsync(book), Times.Once);
    }

    [Test]
    [AutoData]
    public async Task PutBookByIdAsync_ValidModel_CallsService(Book book, int id)
    {
        var response = await Sut.PutBookByIdAsync(id, book);

        _mockBookService.Verify(bs => bs.UpdateBookAsync(id, book), Times.Once);

        response.Should().Match<OkResult>(r => r.StatusCode == StatusCodes.Status200OK);
    }

    [Test]
    [AutoData]
    public async Task PutBookByIdAsync_InvalidModel_ReturnsValidationErrors(Book book, int id,
        ValidationResult validationResult)
    {
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var response = await Sut.PutBookByIdAsync(id, book);
        var validationMessage = string.Join('|', validationResult.Errors.Select(e => e.ErrorMessage));
        _mockBookService.Verify(bs => bs.UpdateBookAsync(It.IsAny<int>(), It.IsAny<Book>()), Times.Never);
        response.Should().Match<BadRequestObjectResult>(r =>
            r.StatusCode == StatusCodes.Status400BadRequest && r.Value.As<string>() == validationMessage);
    }

    [Test]
    [AutoData]
    public async Task PutBookByIdAsync_ServiceThrowsException_PropagatesException(Book book, int id,
        Exception exception)
    {
        _mockBookService.Setup(bs => bs.UpdateBookAsync(It.IsAny<int>(), It.IsAny<Book>())).ThrowsAsync(exception);
        Func<Task> act = async () => await Sut.PutBookByIdAsync(id, book);

        await act.Should().ThrowAsync<Exception>().WithMessage(exception.Message);
        _mockBookService.Verify(bs => bs.UpdateBookAsync(id, book), Times.Once);
    }

    [Test]
    [AutoData]
    public async Task GetBookByIdAsync_ValidModel_CallsServiceAndReturnsBook(Book book, int id)
    {
        _mockBookService.Setup(bs => bs.GetBookByIdAsync(It.IsAny<int>())).ReturnsAsync(book);

        var response = await Sut.GetBookByIdAsync(id);

        _mockBookService.Verify(bs => bs.GetBookByIdAsync(id), Times.Once);

        response.Result.Should()
            .Match<OkObjectResult>(r => r.StatusCode == StatusCodes.Status200OK && r.Value.As<Book>() == book);
    }

    [Test]
    [AutoData]
    public async Task GetBookByIdAsync_ServiceThrowsException_PropagatesException(int id, Exception exception)
    {
        _mockBookService.Setup(bs => bs.GetBookByIdAsync(It.IsAny<int>())).ThrowsAsync(exception);
        Func<Task> act = async () => await Sut.GetBookByIdAsync(id);

        await act.Should().ThrowAsync<Exception>().WithMessage(exception.Message);
    }

    [Test]
    [AutoData]
    public async Task DeleteBookByIdAsync_CallsService(int id)
    {
        var response = await Sut.DeleteBookByIdAsync(id);

        _mockBookService.Verify(bs => bs.DeleteBookAsync(id), Times.Once);

        response.Should().Match<OkResult>(r => r.StatusCode == StatusCodes.Status200OK);
    }

    [Test]
    [AutoData]
    public async Task DeleteBookByIdAsync_ServiceThrowsException_PropagatesException(int id, Exception exception)
    {
        _mockBookService.Setup(bs => bs.DeleteBookAsync(It.IsAny<int>())).ThrowsAsync(exception);
        Func<Task> act = async () => await Sut.DeleteBookByIdAsync(id);

        await act.Should().ThrowAsync<Exception>().WithMessage(exception.Message);
    }
}