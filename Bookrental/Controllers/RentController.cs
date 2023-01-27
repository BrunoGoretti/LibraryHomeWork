using Bookrental.Data;
using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace Bookrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly ApiContext _context;

        public RentController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost("Rent")]
        public async Task<ActionResult<List<BookModel>>> Rent(int bookId, int customerId)
        {
            var book = _context.DbBook.Find(bookId);
            var customer = _context.DbCustomer.Find(customerId);

            if (book == null)
            {
                return BadRequest("Book not found.");
            }
            else if (customer == null)
            {
                return BadRequest("Customer not found.");
            }

            book.RentedDetails = $"Rented by {customer.CustomerName} on {DateTime.Now}";
            _context.DbBook.Update(book);

            var rentedBooks = _context.DbBook
                            .Where(b => b.BookId == customerId)
                            .ToList();
            customer.RentedBooks = rentedBooks;
            customer.RentedBooks.Add(book);
            _context.DbCustomer.Update(customer);


            await _context.SaveChangesAsync();


            return Ok(book);
        }

        [HttpPost("Return")]
        public async Task<ActionResult<List<BookModel>>> Return(int bookId, int customerId)
        {
            var book = _context.DbBook.Find(bookId);
            var customer = _context.DbCustomer.Find(customerId);

            if (book == null)
            {
                return BadRequest("Book not found.");
            }

            var rentedBooks = _context.DbBook
                .Where(b => b.BookId == customerId)
                .ToList();
            customer.RentedBooks = rentedBooks;
            customer.RentedBooks.Remove(book);
            _context.DbCustomer.Update(customer);

            book.RentedDetails = null;
            _context.DbBook.Update(book);
            await _context.SaveChangesAsync();

            return Ok(book);
        }
    }
}
