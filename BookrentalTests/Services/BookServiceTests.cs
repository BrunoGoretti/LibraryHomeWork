using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit.Sdk;
using Microsoft.EntityFrameworkCore;
using Bookrental.Data;
using Bookrental.Models;

namespace Bookrental.Services.Tests
{
    [TestClass()]
    public class BookServiceTests
    {
        private BookService _bookService;
        private DbContextOptions<ApiContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            _options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "AddBook")
                .Options;

            _bookService = new BookService(new ApiContext(_options));
        }

        [TestMethod]
        public async Task AddBook_WithValidInput_ReturnsBookModel()
        {
            // arrange  
            var str = "The Hitchhiker's Guide to the Galaxy";

            // Act
            var result = await _bookService.AddBook(str);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(str, result.BookName);
        }

        [TestMethod]
        public async Task AddBook_WithNullInput_ThrowsArgumentNullException()
        {
            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                _bookService.AddBook(null));
        }

        [TestMethod]
        public async Task AddBook_WithValidInput_AddsBookToContext()
        {
            // arrange  
            var str = "The Hitchhiker's Guide to the Galaxy";

            // Act
            await _bookService.AddBook(str);

            // Assert
            var context = new ApiContext(_options);
            var books = context.DbBook.ToList();
            Assert.AreEqual(1, books.Count);
            Assert.AreEqual(str, books[0].BookName);
        }

        [TestMethod]
        public async Task GetBook_ReturnsExpectedBook()
        {
            // Arrange
            var context = new ApiContext(_options);
            var bookId = 1;
            var expectedBook = new BookModel { BookId = bookId, RentedDetails = "Customer name" };
            context.DbBook.Add(expectedBook);

            var service = new BookService(context);

            // Act
            var result = await service.GetBook(bookId);

            // Assert
            Assert.AreEqual(expectedBook, result);
        }

        [TestMethod]
        public async Task GetBook_ThrowsExceptionForInvalidBookId()
        {
            // Arrange
            var context = new ApiContext(_options);
            var bookId = 1;
            var expectedBook = new BookModel { BookId = bookId, RentedDetails = "Customer name" };
            context.DbBook.Add(expectedBook);

            var service = new BookService(context);

            // Act
            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => service.GetBook(2));

            // Assert
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public async Task GetBooks_ShouldReturnCorrectListOfBooks()
        {
            // Arrange
            var book1 = new BookModel { BookName = "Book 1", RentedDetails = "Rented" };
            var book2 = new BookModel { BookName = "Book 2", RentedDetails = "Not Rented" };
            var context = new ApiContext(_options);
                context.DbBook.Add(book1);
                context.DbBook.Add(book2);
                context.SaveChanges();

            // Act
            var result = await _bookService.GetBooks();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Book 1", result[0].BookName);
            Assert.AreEqual("Rented", result[0].RentedDetails);
            Assert.AreEqual("Book 2", result[1].BookName);
            Assert.AreEqual("Not Rented", result[1].RentedDetails);
        }

        [TestMethod]
        public async Task UpdateBook_ShouldUpdateCorrectOfBooks()
        {
            // Arrange
            var book = new BookModel { BookName = "Book 1", RentedDetails = "Rented(Unchangeable)" };
            var context = new ApiContext(_options);
                context.DbBook.Add(book);
                context.SaveChanges();

            var updatedBook = new BookModel { BookId = book.BookId, 
                BookName = "Updated Book", RentedDetails = "Not Rented" };

            // Act
            var result = await _bookService.UpdateBook(updatedBook);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Book", result.BookName);
            Assert.AreEqual("Rented(Unchangeable)", result.RentedDetails);
        }

        [TestMethod]
        
        public async Task UpdateBook_BookNotFound()
        {
            // Arrange
            var book = new BookModel { BookId = 1, BookName = "Book 1", RentedDetails = "Rented(Unchangeable)" };

            // Act
            try
            {
                await _bookService.UpdateBook(book);

                // Assert
                Assert.Fail("Exception was not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.AreEqual("Book not found.", ex.Message);
            }
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
        public async Task DeleteBook_ShouldDeleteBookSuccessfully()
        {
            // Arrange
            var book = new BookModel { BookId = 1, BookName = "Book 1", RentedDetails = "Rented" };
            await _bookService.AddBook(book.BookName);

            // Act
            var result = await _bookService.DeleteBook(1);

            // Assert
            var books = await _bookService.GetBooks();
            Assert.IsFalse(books.Any(b => b.BookId == 1));
            Assert.AreEqual(book.BookName, result.BookName);
        }

        [TestMethod]
        public async Task DeleteBook_ShouldThrowExceptionWhenBookNotFound()
        {
            // Act
            try
            {
                await _bookService.DeleteBook(1);

                // Assert
                Assert.Fail("Exception was not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.AreEqual("Book not found.", ex.Message);
            }
        }
    }
}