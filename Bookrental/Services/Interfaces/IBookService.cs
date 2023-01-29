using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Services.Interfaces
{
    public interface IBookService
    {
        public Task<ActionResult<List<BookModel>>> AddBook(string bookName);
    }
}
