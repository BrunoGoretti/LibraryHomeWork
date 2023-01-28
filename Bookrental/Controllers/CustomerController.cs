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
            var customer = new CustomerModel {CustomerName = customerName};
            _context.DbCustomer.Add(customer);
            _context.SaveChanges();
            return Ok(await _context.DbCustomer.ToListAsync());
        }

        [HttpGet("GetCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> GetCustomer(int customerId)
        {
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
            var customer = await _context.DbCustomer.ToListAsync();
            await _context.CustomerModel
                            .Select(x => x.RentedBooks)
                            .ToListAsync();

            foreach (var c in customer)
            {
                c.RentedBooks ??= new List<BookModel>();
            }
            return Ok(customer);
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

        [HttpDelete("DeleteCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> DeleteCustomer(int customerId)
        {

            var customer = _context.DbCustomer.Find(customerId);
            if (customer == null)
            {
                return BadRequest("Customer not found.");
            }

            var rentedBooks = _context.DbBook
                                .Where(b => b.RentedDetails != null && b.RentedDetails.Contains(customer.CustomerName))
                                .ToList();
            if (rentedBooks.Count > 0)
            {
                return BadRequest("Customer cannot be removed as they are currently renting books.");
            }

            _context.DbCustomer.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
