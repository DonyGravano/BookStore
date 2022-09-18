using BookStore.Application.Interfaces;
using BookStore.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IValidator<Book> _bookValidator;

    public BookController(IBookService bookService, IValidator<Book> bookValidator)
    {
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        _bookValidator = bookValidator ?? throw new ArgumentNullException(nameof(bookValidator));
    }

    [HttpPost]
    [Route("books")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> PostBook(Book book)
    {
        var validationResult = await _bookValidator.ValidateAsync(book);
        if (!validationResult.IsValid)
            return BadRequest(string.Join('|', validationResult.Errors.Select(e => e.ErrorMessage)));
        var newBookId = await _bookService.CreateBookAsync(book);
        return new ObjectResult(newBookId)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    [HttpGet]
    [Route("books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Book>>> BooksAsync()
    {
        return Ok((await _bookService.GetBooksAsync()).OrderByDescending(b => b.Title));
    }

    [HttpPut]
    [Route("books/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutBookByIdAsync([FromRoute] int id, [FromBody] Book book)
    {
        var validationResult = await _bookValidator.ValidateAsync(book);
        if (!validationResult.IsValid)
            return BadRequest(string.Join('|', validationResult.Errors.Select(e => e.ErrorMessage)));
        await _bookService.UpdateBookAsync(id, book);
        return Ok();
    }

    [HttpGet]
    [Route("books/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Book>> GetBookByIdAsync([FromRoute] int id)
    {
        return Ok(await _bookService.GetBookByIdAsync(id));
    }

    [HttpDelete]
    [Route("books/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteBookByIdAsync([FromRoute] int id)
    {
        await _bookService.DeleteBookAsync(id);
        return Ok();
    }
}