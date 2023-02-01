using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookrental.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Bookrental.Data;
using Microsoft.Extensions.Options;
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
    }
}