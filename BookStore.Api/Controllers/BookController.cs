using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    public class BookController : Controller
    {
        public BookController()
        {

        }

        [HttpPost]
        [Route("books")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            return new Book();
        }

        [HttpGet]
        [Route("books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyCollection<Book>>> BooksAsync()
        {
            return new List<Book>();
        }

        [HttpPut]
        [Route("books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> PutBookByIdAsync([FromRoute] int id)
        {
            return new Book();
        }

        [HttpGet]
        [Route("books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> GetBookByIdAsync([FromRoute] int id)
        {
            return new Book();
        }

        [HttpDelete]
        [Route("books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBookByIdAsync([FromRoute] int id)
        {
            return Ok();
        }
    }
}
