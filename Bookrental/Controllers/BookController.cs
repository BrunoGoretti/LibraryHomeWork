using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("AddBook")]
        public async Task<ActionResult> AddBook(string bookName)
        {
            var book = await _bookService.AddBook(bookName);
            return Ok(book);
        }

        [HttpGet("GetBook")]
        public async Task<ActionResult> GetBook(int bookId)
        {
            var result = await _bookService.GetBook(bookId);
            return Ok(result);
        }

        [HttpGet("GetBooks")]
        public async Task<ActionResult> GetBooks()
        {
            var result = await _bookService.GetBooks();
            return Ok(result);
        }

        [HttpPut("UpdateBook")]
        public async Task<ActionResult> UpdateBook(BookModel book)
        {
            var result = await _bookService.UpdateBook(book);
            return Ok(result);
        }

        [HttpDelete("DeleteBook/{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBook(id);
            return Ok(result);
        }
    }
}
