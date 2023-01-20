using Bookrental.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookrental.Data
{
    public class CustomerApiContext : DbContext
    {
        public DbSet<CustomerModel> DbCustomer { get; set; }
        public CustomerApiContext(DbContextOptions<CustomerApiContext> options)
            : base(options) { }
        public DbSet<CustomerModel> CustomerModel => Set<CustomerModel>();
    }
}

