using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookrental.Services
{
    public class BookService : IBookService
    {
        private readonly ApiContext _context;

        public BookService(ApiContext context)
        {
            _context = context;
        }

        public Task<ActionResult<List<BookModel>>> AddBook(string bookName)
        {
            var books = new BookModel { BookName = bookName};
            _context.DbBook.Add(books);
            _context.SaveChanges();
            return Ok( _context.DbBook.Where(x => x.BookName == bookName).FirstOrDefaultAsync());
        }

        public Task<ActionResult<List<BookModel>>> GetBook(int bookId)
        {
            var book = _context.DbBook.Find(bookId);

            if (book == null)
            { 
                return BadRequest("Book not found.");
            }

            var customerId =  _context.BookModel
                .Where(x => x.BookId == bookId)
                .Select(x => x.RentedDetails)
                .FirstOrDefaultAsync();

            return Ok(book);
        }
    }
}
