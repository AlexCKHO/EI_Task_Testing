using EI_Task.Models;
using EI_Task.Services;
using Microsoft.Extensions.Logging;
using Moq;


namespace Testing
{
    public class BookManagerServiceShould
    {
        private BookManagerService _bookManagerService;
        private Mock<ILogger<IUserManagerService>> _mockLogger;
        private Mock<ILibraryService<Book>> _mockBookService;
        private Mock<ILibraryService<Branch>> _mockBranchService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<IUserManagerService>>();
            _mockBookService = new Mock<ILibraryService<Book>>();
            _mockBranchService = new Mock<ILibraryService<Branch>>();

            _bookManagerService = new BookManagerService(_mockLogger.Object, _mockBookService.Object, _mockBranchService.Object);

        }

        [Test]
        public async Task GetBranchNameAndId_ShouldReturnCorrectMappings()
        {

            var branches = new List<Branch>
                {
                    new Branch { BranchId = 1, BranchName = "Branch1" },
                    new Branch { BranchId = 2, BranchName = "Branch2" }
                };
            _mockBranchService.Setup(x => x.GetAllAsync()).ReturnsAsync(branches);


            var result = await _bookManagerService.GetBranchNameAndIdAsync();


            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result["Branch1"], Is.EqualTo(1));
            Assert.That(result["Branch2"], Is.EqualTo(2));
        }

        [Test]
        public async Task GetListOfBookAsync_ShouldReturnCorrectNumberOfBooks()
        {

            var mockBooks = new List<Book>
            {
                new Book { BookId = 1, Name = "Book1", PublishedYear = 2020 },
                new Book { BookId = 2, Name = "Book2", PublishedYear = 2019 },
                new Book { BookId = 3, Name = "Book3", PublishedYear = 2021 }
            };

            _mockBookService.Setup(x => x.GetAllAsync()).ReturnsAsync(mockBooks);


            var result = await _bookManagerService.GetListOfBookAsync();

            Assert.That(result.Count, Is.EqualTo(mockBooks.Count));
        }

        [Test]
        public async Task DeleteBookAsync_ShouldDeleteBook()
        {

            int bookId = 1;
            int branchId = 2;
            var mockBook = new Book { BookId = bookId, Name = "Test Book", Availability = true, BranchId = branchId };
            var mockBranch = new Branch { BranchId = branchId };

            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(mockBook);
            _mockBookService.Setup(x => x.DeleteAsync(bookId)).Returns(Task.FromResult(true));
            _mockBranchService.Setup(x => x.GetAsync(branchId)).ReturnsAsync(mockBranch);
            _mockBranchService.Setup(x => x.UpdateAsync(branchId, It.IsAny<Branch>())).Returns(Task.FromResult(true));

            await _bookManagerService.DeleteBookAsync(bookId);

            
            _mockBookService.Verify(x => x.GetAsync(bookId), Times.Exactly(1));
            _mockBookService.Verify(x => x.DeleteAsync(bookId), Times.Once);
            _mockBranchService.Verify(x => x.UpdateAsync(branchId, It.IsAny<Branch>()), Times.Once);
        }

        [Test]
        public async Task GetBookByIdAsync_ShouldReturnCorrectBook_WhenBookExists()
        {

            int bookId = 1;
            var expectedBook = new Book { BookId = bookId, Name = "Test Book" };
            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(expectedBook);


            var result = await _bookManagerService.GetBookByIdAsync(bookId);


            Assert.AreEqual(expectedBook, result);
        }

        [Test]
        public async Task GetBookByIdAsync_ShouldReturnNull_WhenExceptionOccurs()
        {

            int bookId = 1;
            _mockBookService.Setup(x => x.GetAsync(bookId)).ThrowsAsync(new Exception("Some error"));


            var result = await _bookManagerService.GetBookByIdAsync(bookId);


            Assert.IsNull(result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to get book by ID")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)

                )
            );
        }

        [Test]
        public async Task UpdateBookPropertyAsync_ShouldUpdateBookName_WhenPropertyNameIsName()
        {

            int bookId = 1;
            string propertyName = "Name";
            string oldName = "Old Name";
            string newName = "New Name";
            var mockBook = new Book { BookId = bookId, Name = oldName };

            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(mockBook);
            _mockBookService.Setup(x => x.UpdateAsync(bookId, It.IsAny<Book>()))
                           .Callback<int, Book>((id, book) => mockBook.Name = book.Name)
                           .Returns(Task.FromResult(true));


            bool result = await _bookManagerService.UpdateBookPropertyAsync(bookId, propertyName, newName);


            Assert.IsTrue(result);
            Assert.AreEqual(newName, mockBook.Name);
        }

        [Test]
        public async Task UpdateBookPropertyAsync_ShouldUpdateBookYear_WhenPropertyNameIsPublishedYear()
        {
            int bookId = 1;
            string propertyName = "PublishedYear";
            int oldYear = 2000;
            int newYear = 2021;
            var mockBook = new Book { BookId = bookId, PublishedYear = oldYear };

            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(mockBook);
            _mockBookService.Setup(x => x.UpdateAsync(bookId, It.IsAny<Book>()))
                           .Callback<int, Book>((id, book) => mockBook.PublishedYear = book.PublishedYear)
                           .Returns(Task.FromResult(true));

            bool result = await _bookManagerService.UpdateBookPropertyAsync(bookId, propertyName, newYear);

            Assert.IsTrue(result);
            Assert.AreEqual(newYear, mockBook.PublishedYear);
        }

        [Test]
        public async Task UpdateBookPropertyAsync_ShouldUpdateAvailability_WhenPropertyNameIsAvailability()
        {
            int bookId = 1;
            string propertyName = "Availability";
            bool oldAvailability = false;
            bool newAvailability = true;
            var mockBook = new Book { BookId = bookId, Availability = oldAvailability };

            _mockBookService.Setup(x => x.GetAsync(bookId)).ReturnsAsync(mockBook);
            _mockBookService.Setup(x => x.UpdateAsync(bookId, It.IsAny<Book>()))
                           .Callback<int, Book>((id, book) => mockBook.Availability = book.Availability)
                           .Returns(Task.FromResult(true));

            bool result = await _bookManagerService.UpdateBookPropertyAsync(bookId, propertyName, newAvailability);

            Assert.IsTrue(result);
            Assert.AreEqual(newAvailability, mockBook.Availability);
        }

    }
}
