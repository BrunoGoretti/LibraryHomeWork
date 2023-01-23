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

        //[HttpPost("rent")]
        //public async Task<ActionResult<List<RentBookModel>>> RentBook(RentBookModel rent)
        //{
        //    if (books.Contains(rent))
        //    {
        //        rent._context = true;
        //        Console.WriteLine("Book rented successfully!");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Sorry, the book is not available for rent.");
        //    }
        //}

    }
}
