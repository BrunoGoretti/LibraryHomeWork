using Bookrental.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookrental.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<BookModel> DbBook { get; set; }
        public DbSet<CustomerModel> DbCustomer { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options) { }
        public DbSet<BookModel> BookModel => Set<BookModel>();
    }
}
