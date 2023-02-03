using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookrentalTests.Services
{
    [TestClass()]
    public class CustomerServiceTests
    {
        private CustomerService _customerService;
        private ApiContext _context;
        private DbContextOptions<ApiContext> _options;


        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            _options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "AddCustomer")
            .Options;

            _context = new ApiContext(_options);
            _customerService = new CustomerService(new ApiContext(_options));
        }

        [TestMethod]
        public async Task AddCustomer_WithValidInput_ReturnsCustomerModel()
        {
            // arrange  
            var str = "Mr. White";

            // Act
            var result = await _customerService.AddCustomer(str);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(str, result.CustomerName);
        }

        [TestMethod]
        public async Task AddCustomer_WithValidInput_AddsCustomerToContext()
        {
            // arrange  
            var str = "Mr. White";

            // Act
            await _customerService.AddCustomer(str);

            // Assert
            var context = new ApiContext(_options);
            var customers = context.DbCustomer.ToList();
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual(str, customers[0].CustomerName);
        }

        [TestMethod]
        public async Task GetCustomer_WithValidId_ReturnsCustomerModel()
        {
            // Arrange
            var customerService = new CustomerService(new ApiContext(_options));
            var customerName = "Mr. White";
            var addedCustomer = await customerService.AddCustomer(customerName);
            var customerId = addedCustomer.CustomerId;

            // Act
            var result = await customerService.GetCustomer(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customerId, result.CustomerId);
            Assert.AreEqual(customerName, result.CustomerName);
            Assert.IsNotNull(result.RentedBooks);
        }

        [TestMethod]
        public async Task GetCustomer_WithInvalidId_ThrowsException()
        {
            // Arrange
            var customerService = new CustomerService(new ApiContext(_options));
            var customerId = -1;

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => customerService.GetCustomer(customerId), "Customer not found.");
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsListOfCustomers()
        {
            // Arrange
            var customerService = new CustomerService(new ApiContext(_options));
            var customerName1 = "Mr. White";
            var customerName2 = "Ms. Black";
            await customerService.AddCustomer(customerName1);
            await customerService.AddCustomer(customerName2);

            // Act
            var result = await customerService.GetCustomers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(c => c.CustomerName == customerName1));
            Assert.IsTrue(result.Any(c => c.CustomerName == customerName2));
            foreach (var customer in result)
            {
                Assert.IsNotNull(customer.RentedBooks);
            }
        }

        [TestMethod]
        public async Task UpdateCustomer_WithValidInput_UpdatesCustomer()
        {
            // Arrange
            var customerService = new CustomerService(new ApiContext(_options));
            var customerName = "Mr. White";
            var customer = await customerService.AddCustomer(customerName);
            var newCustomerName = "Ms. Black";

            // Act
            var result = await customerService.UpdateCustomer(new CustomerModel
            {
                CustomerId = customer.CustomerId,
                CustomerName = newCustomerName
            });

            // Assert
            Assert.AreEqual(newCustomerName, result.CustomerName);

            var updatedCustomer = await customerService.GetCustomer(customer.CustomerId);
            Assert.AreEqual(newCustomerName, updatedCustomer.CustomerName);
        }

        [TestMethod]
        public async Task DeleteCustomer_WithValidInput_DeletesCustomerFromContext()
        {
            // arrange  
            var str = "Mr. White";
            var customer = await _customerService.AddCustomer(str);

            // Act
            await _customerService.DeleteCustomer(customer.CustomerId);

            // Assert
            var context = new ApiContext(_options);
            var customers = context.DbCustomer.ToList();
            Assert.AreEqual(0, customers.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Customer not found.")]
        public async Task DeleteCustomer_WithInvalidId_ThrowsException()
        {
            // Act
            await _customerService.DeleteCustomer(0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Customer cannot be removed as they are currently renting books.")]
        public async Task DeleteCustomer_WithCustomerRentingBooks_ThrowsException()
        {
            // arrange  
            var str = "Mr. White";
            var customer = await _customerService.AddCustomer(str);

            // Arrange
            var book = new BookModel { BookId = 1, BookName = "The Shining", RentedDetails = str };
            _context.DbBook.Add(book);
            await _context.SaveChangesAsync();

            // Act
            await _customerService.DeleteCustomer(customer.CustomerId);
        }
    }
}
