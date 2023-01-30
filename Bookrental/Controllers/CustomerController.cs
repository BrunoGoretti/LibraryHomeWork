using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services;
using Bookrental.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Bookrental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly ICustomerService _customerService;

        public CustomerController(ApiContext context, ICustomerService customerService)
        {
            _customerService = customerService;
            _context = context;
        }

        [HttpPost("AddCustomer")]
        public async Task<ActionResult> AddCustomer(string customerName)
        {
            var customer = await _customerService.AddCustomer(customerName);
            return Ok(customer);
        }

        [HttpGet("GetCustomer")]
        public async Task<ActionResult> GetCustomer(int customerId)
        {
            var result = await _customerService.GetCustomer(customerId);
            return Ok(result);
        }

        [HttpGet("GetCustomers")]
        public async Task<ActionResult> GetCustomers()
        {
            var result = await _customerService.GetCustomers();
            return Ok(result);
        }

        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> UpdateCustomer(CustomerModel customer)
        {
            var result = await _customerService.UpdateCustomer(customer);
            return Ok(result);
        }

        [HttpDelete("DeleteCustomer")]
        public async Task<ActionResult<List<CustomerModel>>> DeleteCustomer(int customerId)
        {
            var result = await _customerService.DeleteCustomer(customerId);
            return Ok(result);
        }
    }
}
