using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookModel> AddBook(string bookName);
        Task<BookModel> GetBook(int bookId);
        Task<List<BookModel>> GetBooks();
        Task<BookModel> UpdateBook(BookModel book);
        Task<BookModel> DeleteBook(int id);
    }
}
