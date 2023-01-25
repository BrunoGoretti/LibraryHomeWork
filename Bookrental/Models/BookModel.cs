using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookrental.Models
{
    public class BookModel
    {
        [Key]
        public int BookId { get; set; }
        public string? BookName { get; set; }
        [JsonIgnore]
        public List<CustomerModel> RentedCustomer { get; set; }
    }
}
