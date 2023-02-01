using Bookrental.Models;
using Bookrental.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly IRentService _rentService;

        public RentController(IRentService rentService)
        {
            _rentService = rentService;
        }

        [HttpPost("Rent")]
        public async Task<ActionResult<BookModel>> Rent(int bookId, int customerId)
        {
            var result = await _rentService.Rent(bookId, customerId);

            return Ok(result);
        }

        [HttpPost("Return")]
        public async Task<ActionResult<List<BookModel>>> Return(int bookId, int customerId)
        {
            var result = await _rentService.Return(bookId, customerId);

            return Ok(result);
        }
    }
}
