using System.ComponentModel.DataAnnotations;

namespace Bookrental.Models
{
    public class CustomerModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

    }
}
