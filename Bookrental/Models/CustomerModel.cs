using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookrental.Models
{
    public class CustomerModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        [JsonIgnore]
        public List<BookModel> RentedBooks { get; set; }
    }
}
