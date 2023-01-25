using Bookrental.Data;
using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApiContext _context;
        public BookController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost("AddBook")]
        public async Task<ActionResult<List<BookModel>>> AddBook(string bookName)
        {
            var books = new BookModel { BookName = bookName,};
            _context.DbBook.Add(books);
            _context.SaveChanges();
            return Ok(await _context.DbBook.ToListAsync());
        }

        [HttpGet("GetOneBooks")]
        public async Task<ActionResult<List<BookModel>>> GetOneBook(int bookId)
        {
            var book = _context.DbBook.Find(bookId);

            if (book == null)
            {
                return BadRequest("Book not found.");
            }

            var customerId = await _context.BookModel
                .Where(x => x.BookId == bookId)
                .Select(x => x.RentedCustomer)
                .FirstOrDefaultAsync();

            return Ok(new { book, customerId });
        }

        [HttpGet("GetBooks")]
        public async Task<ActionResult<List<BookModel>>> GetBooks()
        {
            return Ok(await _context.DbBook.ToListAsync());
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

            return Ok(await _context.DbBook.ToListAsync());
        }

        [HttpDelete("{id} DeleteBook")]
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
