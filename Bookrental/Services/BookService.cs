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

        public async Task<BookModel> AddBook(string bookName)
        {
            var book = new BookModel { BookName = bookName };
            _context.DbBook.Add(book);
            _context.SaveChanges();
            return await _context.DbBook.Where(x => x.BookName == bookName).FirstOrDefaultAsync();
        }

        public async Task<BookModel> GetBook(int bookId)
        {
            var book = _context.DbBook.Find(bookId);

            if (book == null)
            {
               throw new Exception ("Book not found.");
            }

            var customerId = _context.BookModel
                .Where(x => x.BookId == bookId)
                .Select(x => x.RentedDetails)
                .FirstOrDefaultAsync();

            return book;
        }
    }
}
