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
        private readonly CustomerApiContext _context;

        public CustomerController(CustomerApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<CustomerModel>>> AddCustomer(CustomerModel customer)
        {
            _context.CustomerModel.Add(customer);
            await _context.SaveChangesAsync();
            return Ok(await _context.CustomerModel.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerModel>>> GetCustomer()
        {
            return Ok(await _context.CustomerModel.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<CustomerModel>>> UpdateCustomer(CustomerModel customer)
        {
            var dbCustomer = await _context.CustomerModel.FindAsync(customer.Id);
            if (dbCustomer == null)
            {
                return BadRequest("Customer not found.");
            }

            dbCustomer.CustomerName = customer.CustomerName;

            await _context.SaveChangesAsync();

            return Ok(await _context.CustomerModel.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<CustomerModel>>> DeleteCustomer(int id)
        {
            var dbCustomer = await _context.CustomerModel.FindAsync(id);
            if (dbCustomer == null)
            {
                return BadRequest("Customer not found.");
            }

            _context.CustomerModel.Remove(dbCustomer);
            await _context.SaveChangesAsync();

            return Ok(await _context.CustomerModel.ToListAsync());
        }
    }
}
