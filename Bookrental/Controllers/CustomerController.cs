using Bookrental.Data;
using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Bookrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApiContext _context;

        public CustomerController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost("AddCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> AddCustomer(string customerName)
        {
            var customer = new CustomerModel
            {
                CustomerName = customerName,
                RentedBooks = new List<BookModel>()
            };
            _context.DbCustomer.Add(customer);
            _context.SaveChanges();
            return Ok(customer);
        }

        [HttpGet("GetOneCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> GetOneCustomer(int customerId)
        {
            //var customer = _context.DbCustomer.Find(customerId);

            var customer = await _context.DbCustomer
                     .Include(c => c.RentedBooks)
                     .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return BadRequest("Customer not found.");
            }

            customer.RentedBooks = await _context.CustomerModel
                            .Where(x => x.CustomerId == customerId)
                            .Select(x => x.RentedBooks)
                            .FirstOrDefaultAsync();

            return Ok(customer);
        }

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

            return Ok(dbCustomer);
        }

        [HttpDelete("DeleteCustomer/{id}")]
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
