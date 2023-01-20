using Bookrental.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookrental.Data
{
    public class BookApiContext : DbContext
    {
        public DbSet<BookModel> DbBook { get; set; }
        public BookApiContext(DbContextOptions<BookApiContext> options)
            : base(options) { }
        public DbSet<BookModel> BookModel => Set<BookModel>();
    }
}
