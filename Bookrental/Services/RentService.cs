using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services.Interfaces;

namespace Bookrental.Services
{
    public class RentService : IRentService
    {
        private readonly ApiContext _context;

        public RentService(ApiContext context)
        {
            _context = context;
        }

        public async Task<BookModel> Rent(int bookId, int customerId)
        {
            var book = _context.DbBook.Find(bookId);
            var customer = _context.DbCustomer.Find(customerId);

            if (book == null)
            {
                throw new Exception ("Book not found.");
            }
            else if (customer == null)
            {
                throw new Exception ("Customer not found.");
            }

            //A book can only be rented put by one customer at any point in time 

            if (book.RentedDetails != null)
            {
                throw new Exception ("Book is already rented.");
            }

            book.RentedDetails = $"Rented by {customer.CustomerName} on {DateTime.Now}";
            _context.DbBook.Update(book);

            // A customer is only allowed to rent up to two books simultaneously

            var rentedBooks = _context.DbBook
                            .Where(b => b.RentedDetails != null && b.RentedDetails
                            .Contains(customer.CustomerName))
                            .ToList();

            if (rentedBooks.Count >= 2)
            {
                throw new Exception ("Customer has already rented the maximum of two books.");
            }

            customer.RentedBooks = rentedBooks;
            customer.RentedBooks.Add(book);
            _context.DbCustomer.Update(customer);

            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<BookModel> Return(int bookId, int customerId)
        {
            var book = _context.DbBook.Find(bookId);
            var customer = _context.DbCustomer.Find(customerId);

            if (book == null)
            {
                throw new Exception("Book not found.");
            }

            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }

            var rentedBooks = _context.DbBook
                .Where(b => b.BookId == customerId)
                .ToList();
            customer.RentedBooks = rentedBooks;
            customer.RentedBooks.Remove(book);
            _context.DbCustomer.Update(customer);

            book.RentedDetails = null;
            _context.DbBook.Update(book);

            if (book.RentedDetails == null)
            {
                book.RentedDetails ??= "";
            }

            await _context.SaveChangesAsync();

            return book;
        }
    }
}
