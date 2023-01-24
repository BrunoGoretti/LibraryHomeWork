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
            var customer = new CustomerModel { CustomerName = customerName, RentedBooks = null };
            _context.DbCustomer.Add(customer);
            _context.SaveChanges();
            return Ok(await _context.DbCustomer.ToListAsync());
        }

        [HttpGet("RentedBooks")]
        public async Task<ActionResult<List<CustomerModel>>> GetCustomersBooks(int customerId)
        {
            var bookId = await _context.CustomerModel
                .Where(x => x.CustomerId == customerId)
                .Select(x => x.RentedBooks)
                .FirstOrDefaultAsync();
            if (bookId == null)
            {
                return NotFound();
            }
            return Ok(bookId);

            //var customer = _context.DbCustomer.Find(id);
            //var rentedBooks = _context.DbBook
            //                .Where(b => b.BookId == id)
            //                .ToList();
            //customer.RentedBooks = rentedBooks;

            //if (customer == null)
            //{
            //    return BadRequest("Customer not found.");
            //}

            //return Ok(customer);

            //var customer = _context.DbCustomer.FirstOrDefault(c => c.CustomerId == customerId);
            //if (customer == null)
            //{
            //    return NotFound();
            //}

            //var books = _context.DbBook.Where(b => b.BookId == customerId).ToList();
            //var viewModel = new CustomerModel
            //{
            //    CustomerId = customer.CustomerId,
            //    CustomerName = customer.CustomerName,
            //    RentedBooks = books
            //};

            //return Ok(viewModel);
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
