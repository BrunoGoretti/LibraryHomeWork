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

        [HttpPost]
        public async Task<ActionResult<List<BookModel>>> AddBook(BookModel book)
        {
            _context.DbBook.Add(book);
            await _context.SaveChangesAsync();
            return Ok(await _context.DbBook.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult<List<BookModel>>> GetBooks()
        {
            return Ok(await _context.DbBook.ToListAsync());
        }

        [HttpPut]
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

        [HttpDelete("{id}")]
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
