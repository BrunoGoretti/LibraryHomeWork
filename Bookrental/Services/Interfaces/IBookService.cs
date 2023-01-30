using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookModel> AddBook(string bookName);
        Task<BookModel> GetBook(int bookId);

    }
}
