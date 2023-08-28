using EI_Task.Data.Repositories;
using EI_Task.Models;
using EI_Task.Services;
using Moq;


namespace Testing
{
    public class AccountsServiceTests
    {
        private AccountsService _accountsService;
        private Mock<ILibraryRepository<Account>> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ILibraryRepository<Account>>();
            _accountsService = new AccountsService(_mockRepository.Object);
        }

        [Test]
        public async Task LoginAsync_ReturnsUserId_WhenCredentialsAreValid()
        {

            var mockAccounts = new List<Account>
            {
                new Account { AccountId = 1, Email = "test@email.com", Password = "password", UserId = 10 }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(mockAccounts);


            var result = await _accountsService.LoginAsync("test@email.com", "password");


            Assert.That(result, Is.EqualTo(10));
        }

        [Test]
        public async Task LoginAsync_ReturnsMinusOne_WhenCredentialsAreInvalid()
        {

            var mockAccounts = new List<Account>
            {
                new Account { AccountId = 1, Email = "test@email.com", Password = "password", UserId = 10 }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(mockAccounts);


            var result = await _accountsService.LoginAsync("wrong@email.com", "wrongpassword");


            Assert.That(result, Is.EqualTo(-1));
        }
    }
}
