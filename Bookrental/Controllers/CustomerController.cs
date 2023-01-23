using Bookrental.Data;
using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApiContext _context;
        //private readonly List<BookModel> _book;


        public CustomerController(ApiContext context)
        {
            _context = context;
        }

        //public ClientbookController(List<BookModel> bookcontext)
        //{
        //    _book = new List<BookModel>();
        //}

        [HttpPost("AddCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> AddCustomer(CustomerModel customer)
        {
            _context.DbCustomer.Add(customer);
            await _context.SaveChangesAsync();
            return Ok(await _context.DbCustomer.ToListAsync());
        }

        //[HttpGet("rented books")]
        //public async Task<ActionResult<List<CustomerModel>>> GetCustomersBooks(int customerId)
        //{

        //}

        [HttpGet("GetCustomers")]
        public async Task<ActionResult<List<CustomerModel>>> GetCustomers()
        {
            return Ok(await _context.DbCustomer.ToListAsync());
        }

        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> UpdateCustomer(CustomerModel customer)
        {
            var dbCustomer = await _context.DbCustomer.FindAsync(customer.CustomerId);
            if (dbCustomer == null)
            {
                return BadRequest("Customer not found.");
            }

            dbCustomer.CustomerName = customer.CustomerName;

            await _context.SaveChangesAsync();

            return Ok(await _context.DbCustomer.ToListAsync());
        }

        [HttpDelete("{id} DeleteCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> DeleteCustomer(int id)
        {
            var dbCustomer = await _context.DbCustomer.FindAsync(id);
            if (dbCustomer == null)
            {
                return BadRequest("Customer not found.");
            }

            _context.DbCustomer.Remove(dbCustomer);
            await _context.SaveChangesAsync();

            return Ok(await _context.DbCustomer.ToListAsync());
        }
    }
}
