using Bookrental.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerModel> AddCustomer(string customerName);
        Task<CustomerModel> GetCustomer(int customerId);
        Task<List<CustomerModel>> GetCustomers();
        Task<CustomerModel> UpdateCustomer(CustomerModel customer);
        Task<CustomerModel> DeleteCustomer(int customerId);
    }
}
