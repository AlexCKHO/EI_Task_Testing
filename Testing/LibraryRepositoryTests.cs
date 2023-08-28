using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using EI_Task.Data.Repositories;
using System.Collections.Generic;
using EI_Task.Data;
using EI_Task.Models;

namespace Testing
{
    public class LibraryRepositoryTests
    {
        private LibraryDbContext _context;
        private LibraryRepository<Book> _sut; 


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase("LibraryTestDB").Options;

            _context = new LibraryDbContext(options);
            if (_context.Books != null)
            {
                _context.Books.RemoveRange(_context.Books);
            }
            _sut = new LibraryRepository<Book>(_context);
        }

        [Test]
        public void Add_AddsBookToDbSet()
        {
            var testBook = new Book { Name = "Test Book", PublishedYear = 2022, Availability = true };

            _sut.Add(testBook);
            _sut.SaveAsync().Wait();

            var result = _sut.FindAsync(1).Result;

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(testBook.Name));
        }

        [Test]
        public void AddRange_AddsBooksToDbSet()
        {
            var testBooks = new List<Book>
            {
                new Book { BookId = 1, Name = "Test Book 1", PublishedYear = 2022, Availability = true },
                new Book { BookId = 2, Name = "Test Book 2", PublishedYear = 2021, Availability = false }
            };

            _sut.AddRange(testBooks);
            _sut.SaveAsync().Wait();

            var result = _sut.GetAllAsync().Result.ToList();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void FindAsync_ReturnsCorrectBook()
        {
            var testBook = new Book { BookId = 1, Name = "Test Book", PublishedYear = 2022, Availability = true };
            _sut.Add(testBook);
            _sut.SaveAsync().Wait();

            var result = _sut.FindAsync(1).Result;

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(testBook.Name));
        }

        [Test]
        public void GetAllAsync_ReturnsAllBooks()
        {
            var testBooks = new List<Book>
            {
                new Book { BookId = 1, Name = "Test Book 1", PublishedYear = 2022, Availability = true },
                new Book { BookId = 2, Name = "Test Book 2", PublishedYear = 2021, Availability = false }
            };

            _sut.AddRange(testBooks);
            _sut.SaveAsync().Wait();

            var result = _sut.GetAllAsync().Result.ToList();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void Remove_RemovesBookFromDbSet()
        {
            var testBook = new Book { BookId = 1, Name = "Test Book", PublishedYear = 2022, Availability = true };
            _sut.Add(testBook);
            _sut.SaveAsync().Wait();

            _sut.Remove(testBook);
            _sut.SaveAsync().Wait();

            var result = _sut.FindAsync(1).Result;

            Assert.IsNull(result);
        }

        [Test]
        public void Update_UpdatesBookInDbSet()
        {
            var testBook = new Book { BookId = 1, Name = "Test Book", PublishedYear = 2022, Availability = true };
            _sut.Add(testBook);
            _sut.SaveAsync().Wait();

            testBook.Name = "Updated Book";
            _sut.Update(testBook);
            _sut.SaveAsync().Wait();

            var result = _sut.FindAsync(1).Result;

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Updated Book"));
        }
    }
}
