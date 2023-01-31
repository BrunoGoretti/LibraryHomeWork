using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Services.Interfaces
{
    public interface IRentService
    {
        Task<BookModel> Rent(int bookId, int customerId);
        Task<BookModel> Return(int bookId, int customerId);
    }
}
