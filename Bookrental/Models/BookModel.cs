using System.ComponentModel.DataAnnotations;

namespace Bookrental.Models
{
    public class BookModel
    {
        [Key]
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? RentedDetails { get; set; }
    }
}
