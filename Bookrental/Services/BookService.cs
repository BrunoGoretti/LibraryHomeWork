using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services.Interfaces;
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

        public async Task<BookModel> AddBook(string bookName)
        {
            if(bookName == null) 
            {
                throw new ArgumentNullException();
            }

            var book = new BookModel { BookName = bookName };
            _context.DbBook.Add(book);

            if (book.RentedDetails == null)
            {
                book.RentedDetails ??= "";
            }

            _context.SaveChanges();
            _context.DbBook?.Where(x => x.BookName == bookName)
                .FirstOrDefaultAsync();
            return book;
        }

        public async Task<BookModel> GetBook(int bookId)
        {
            var book = _context.DbBook.Find(bookId);

            if (book == null)   
            {
               throw new Exception ("Book not found.");
            }

            var customerId = await _context.BookModel
                .Where(x => x.BookId == bookId)
                .Select(x => x.RentedDetails)
                .FirstOrDefaultAsync();

            if (customerId == null)
            {
                customerId ??= "";
            }

            book.RentedDetails = customerId;

            return book;
        }

        public async Task<List<BookModel>> GetBooks()
        {
            var customer = await _context.DbBook.ToListAsync();
            await _context.BookModel
                            .Select(x => x.RentedDetails)
                            .ToListAsync();

            foreach (var book in customer)
            {
                book.RentedDetails ??= "";
            }

            return customer;
        }

        public async Task<BookModel> UpdateBook(BookModel book)
        {
            var dbBook = await _context.DbBook.FindAsync(book.BookId);
            if (dbBook == null)
            {
                throw new Exception ("Book not found.");
            }

            dbBook.BookName = book.BookName;

            if (dbBook.RentedDetails == null)
            {
                dbBook.RentedDetails ??= "";
            }

            await _context.SaveChangesAsync();

            return dbBook;
        }

        public async Task<BookModel> DeleteBook(int id)
        {
            var dbBook = await _context.DbBook.FindAsync(id);
            if (dbBook == null)
            {
                throw new Exception ("Book not found.");
            }

            _context.DbBook.Remove(dbBook);

            if (dbBook.RentedDetails == null)
            {
                dbBook.RentedDetails ??= "";
            }

            await _context.SaveChangesAsync();

            await _context.DbBook.ToListAsync();
            return dbBook;
        }
    }
}
