using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IBookService _bookService;
        public BookController(ApiContext context, IBookService bookService)
        {
            _context = context;
            _bookService = bookService;
        }

        [HttpPost("AddBook")]
        public async Task<ActionResult<List<BookModel>>> AddBook(string bookName)
        {
            var result = _bookService.AddBook(bookName);
            return Ok(result);

        }

        [HttpGet("GetBook")]
        public async Task<ActionResult<List<BookModel>>> GetBook(int bookId)
        {
            var result = _context.DbBook.Find(bookId);
            return Ok(result);
        }

        [HttpGet("GetBooks")]
        public async Task<ActionResult<List<BookModel>>> GetBooks()
        {
            var customer = await _context.DbBook.ToListAsync();
            await _context.BookModel
                            .Select(x => x.RentedDetails)
                            .ToListAsync();

            foreach (var book in customer)
            {
                book.RentedDetails ??= "";
            }

            return Ok(customer);
        }

        [HttpPut("UpdateBook")]
        public async Task<ActionResult<List<BookModel>>> UpdateBook(BookModel book)
        {
            var dbBook = await _context.DbBook.FindAsync(book.BookId);
            if (dbBook == null)
            {
                return BadRequest("Book not found.");
            }

            dbBook.BookName = book.BookName;

            await _context.SaveChangesAsync();

            return Ok(dbBook);
        }

        [HttpDelete("DeleteBook/{id}")]
        public async Task<ActionResult<List<BookModel>>> DeleteBook(int id)
        {
            var dbBook = await _context.DbBook.FindAsync(id);
            if (dbBook == null)
            {
                return BadRequest("Book not found.");
            }

            _context.DbBook.Remove(dbBook);
            await _context.SaveChangesAsync();

            return Ok(await _context.DbBook.ToListAsync());
        }
    }
}
