using Bookrental.Data;
using Bookrental.Models;
using Bookrental.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookrentalTests.Services
{
    [TestClass()]
    public class RentServiceTests
    {
        private RentService _rentService;
        private ApiContext _context;
        private DbContextOptions<ApiContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "BookRentalDb")
                .Options;

            _context = new ApiContext(_options);
            _rentService = new RentService(_context);
        }

        [TestMethod]
        public async Task Rent_RentsBookSuccessfully()
        {
            // Arrange
            var customer = new CustomerModel { CustomerId = 1, CustomerName = "John Doe" };
            var book = new BookModel { BookId = 1, BookName = "The Great Gatsby" };
            _context.DbCustomer.Add(customer);
            _context.DbBook.Add(book);
            await _context.SaveChangesAsync();

            // Act
            var rentedBook = await _rentService.Rent(book.BookId, customer.CustomerId);

            // Assert
            Assert.IsNotNull(rentedBook);
            Assert.AreEqual(book.BookId, rentedBook.BookId);
            Assert.AreEqual(book.BookName, rentedBook.BookName);
            Assert.IsTrue(rentedBook.RentedDetails.Contains(customer.CustomerName));
        }

        [TestMethod]
        public async Task Rent_BookNotFound_ThrowsException()
        {
            // Arrange
            int bookId = 1;
            int customerId = 1;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _rentService.Rent(bookId, customerId), "Book not found.");
        }

        [TestMethod]
        public async Task Rent_CustomerNotFound_ThrowsException()
        {
            // Arrange
            int bookId = 1;
            int customerId = 1;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _rentService.Rent(bookId, customerId), "Customer not found.");
        }

        [TestMethod]
        public async Task Rent_BookAlreadyRented_ThrowsException()
        {
            // Arrange
            int bookId = 1;
            int customerId = 1;
            var book = new BookModel { BookId = bookId, RentedDetails = "Rented" };
            var customer = new CustomerModel { CustomerId = customerId };
            _context.DbBook.Add(book);
            _context.DbCustomer.Add(customer);
            await _context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _rentService.Rent(bookId, customerId), "Book is already rented.");
        }

        [TestMethod]
        public async Task Rent_CustomerHasAlreadyRentedTwoBooks_ThrowsException()
        {
            // Arrange
            int bookId1 = 1;
            int bookId2 = 2;
            int bookId3 = 3;
            int customerId = 1;
            var book1 = new BookModel
            {
                BookId = bookId1,
                RentedDetails =
                $"Rented by customer {customerId} on {DateTime.Now}"
            };
            var book2 = new BookModel
            {
                BookId = bookId2,
                RentedDetails =
                $"Rented by customer {customerId} on {DateTime.Now}"
            };
            var book3 = new BookModel 
            { 
                BookId = bookId3, 
                RentedDetails = 
                $"Rented by customer {customerId} on {DateTime.Now}" 
            };
            var customer = new CustomerModel { CustomerId = customerId };
            _context.DbBook.AddRange(book1, book2, book3);
            _context.DbCustomer.Add(customer);
            await _context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() =>
            _rentService.Rent(bookId3, customerId), "Customer has already rented the maximum of two books.");
        }

        [TestMethod]
        public async Task Return_ValidInput_ReturnsExpectedBook()
        {
            // Arrange
            var customer = new CustomerModel
            {
                CustomerId = 1,
                RentedBooks = new List<BookModel>()
            };

            var book = new BookModel
            {
                BookId = 1,
                RentedDetails = "Rented by Customer 1"
            };

            customer.RentedBooks.Add(book);
            _context.CustomerModel.Add(customer);
            _context.BookModel.Add(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _rentService.Return(book.BookId, customer.CustomerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(book.BookId, result.BookId);
            Assert.AreEqual(string.Empty, result.RentedDetails);

            var updatedCustomer = await _context.CustomerModel
                .Include(c => c.RentedBooks)
                .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

            Assert.IsNotNull(updatedCustomer);
            Assert.IsFalse(updatedCustomer.RentedBooks.Contains(book));
        }

        [TestMethod]
        public async Task Return_BookNotFound_ThrowsException()
        {
            // Arrange
            var customer = new CustomerModel
            {
                CustomerId = 1,
                RentedBooks = new List<BookModel>()
            };

            var book = new BookModel
            {
                BookId = 1,
                RentedDetails = "Rented by Customer 1"
            };

            customer.RentedBooks.Add(book);
            _context.CustomerModel.Add(customer);
            _context.BookModel.Add(book);
            await _context.SaveChangesAsync();

            // Act
            async Task TestReturn() => await _rentService.Return(2, customer.CustomerId);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(TestReturn);
            Assert.AreEqual("Book not found.", exception.Message);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            using (var context = new ApiContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Return_CustomerNotFound_ThrowsException()
        {
            // Arrange
            var customer = new CustomerModel
            {
                CustomerId = 1,
                RentedBooks = new List<BookModel>()
            };

            var book = new BookModel
            {
                BookId = 1,
                RentedDetails = "Rented by Customer 1"
            };

            customer.RentedBooks.Add(book);
            _context.CustomerModel.Add(customer);
            _context.BookModel.Add(book);
            await _context.SaveChangesAsync();

            // Act
            async Task TestReturn() => await _rentService.Return(book.BookId, 2);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(TestReturn);
            Assert.AreEqual("Customer not found.", exception.Message);
        }
    }
}
