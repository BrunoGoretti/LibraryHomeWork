using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookrental.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApiContext _context;

        public CustomerService(ApiContext context)
        {
            _context = context;
        }

        public async Task<CustomerModel> AddCustomer(string customerName)
        {
            var customer = new CustomerModel { CustomerName = customerName };
            _context.DbCustomer.Add(customer);
            _context.SaveChanges();
            return await _context.DbCustomer.Where(x => x.CustomerName == customerName).FirstOrDefaultAsync();
        }

        public async Task<CustomerModel> GetCustomer(int customerId)
        {
            var customer = await _context.DbCustomer
                 .Include(c => c.RentedBooks)
                 .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
            {
                throw new Exception ("Customer not found.");
            }

            customer.RentedBooks = await _context.CustomerModel
                            .Where(x => x.CustomerId == customerId)
                            .Select(x => x.RentedBooks)
                            .FirstOrDefaultAsync();

            return customer;
        }

        public async Task<List<CustomerModel>> GetCustomers()
        {
            var customer = await _context.DbCustomer.ToListAsync();
            await _context.CustomerModel
                            .Select(x => x.RentedBooks)
                            .ToListAsync();

            foreach (var c in customer)
            {
                c.RentedBooks ??= new List<BookModel>();
            }
            return customer;
        }

        public async Task<CustomerModel> UpdateCustomer(CustomerModel customer)
        {
            var dbCustomer = await _context.DbCustomer.FindAsync(customer.CustomerId);
            if (dbCustomer == null)
            {
                throw new Exception ("Customer not found.");
            }

            dbCustomer.CustomerName = customer.CustomerName;

            await _context.SaveChangesAsync();

            return dbCustomer;
        }

        public async Task<CustomerModel> DeleteCustomer(int customerId)
        {

            var customer = _context.DbCustomer.Find(customerId);
            if (customer == null)
            {
                throw new Exception ("Customer not found.");
            }

            var rentedBooks = _context.DbBook
                                .Where(b => b.RentedDetails != null && b.RentedDetails.Contains(customer.CustomerName))
                                .ToList();
            if (rentedBooks.Count > 0)
            {
                throw new Exception ("Customer cannot be removed as they are currently renting books.");
            }

            _context.DbCustomer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }
    }
}
