using EI_Task.Models;
using EI_Task.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;


namespace Testing
{
    public  class UserManagerServiceShould
    {
        private UserManagerService _userManagerService;
        private Mock<ILogger<IUserManagerService>> _mockLogger;
        private Mock<ILibraryService<User>> _mockUserService;
        private Mock<ILibraryService<Account>> _mockAccountService;
        private Mock<ILibraryService<Branch>> _mockBranchService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<IUserManagerService>>();
            _mockUserService = new Mock<ILibraryService<User>>();
            _mockAccountService = new Mock<ILibraryService<Account>>();
            _mockBranchService = new Mock<ILibraryService<Branch>>();

            _userManagerService = new UserManagerService(_mockLogger.Object, _mockUserService.Object, _mockAccountService.Object, _mockBranchService.Object);

        }

        [Test]
        public async Task HasDuplicateEmail_ShouldReturnTrue_WhenEmailExists()
        {

            string existingEmail = "existing@email.com";
            var accounts = new List<Account>
            {
                new Account { Email = existingEmail },
                new Account { Email = "another@email.com" }
            };
            _mockAccountService.Setup(x => x.GetAllAsync()).ReturnsAsync(accounts);


            var result = await _userManagerService.hasDuplicateEmail(existingEmail);


            Assert.IsTrue(result);
        }

        [Test]
        public async Task HasDuplicateEmail_ShouldReturnFalse_WhenEmailDoesNotExist()
        {

            string nonExistingEmail = "nonexisting@email.com";
            var accounts = new List<Account>
            {
                new Account { Email = "existing@email.com" },
                new Account { Email = "another@email.com" }
            };
            _mockAccountService.Setup(x => x.GetAllAsync()).ReturnsAsync(accounts);


            var result = await _userManagerService.hasDuplicateEmail(nonExistingEmail);


            Assert.IsFalse(result);
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


            var result = await _userManagerService.GetBranchNameAndId();


            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result["Branch1"], Is.EqualTo(1));
            Assert.That(result["Branch2"], Is.EqualTo(2));
        }


        [Test]
        public async Task CreateUserAndAccount_ShouldMeetAllConditions()
        {

            Account createdAccount = null;
            User createdUser = null;
            Branch updatedBranch = null;

            _mockAccountService.Setup(x => x.CreateAsync(It.IsAny<Account>())).Callback<Account>(a => createdAccount = a).Returns(Task.FromResult(true));
            _mockUserService.Setup(x => x.CreateAsync(It.IsAny<User>())).Callback<User>(u => createdUser = u).Returns(Task.FromResult(true));

            _mockAccountService.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Account>())).Returns(Task.FromResult(true));
            _mockUserService.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<User>())).Returns(Task.FromResult(true));

            _mockBranchService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(new Branch { NumberOfActiveUsers = 0 });
            _mockBranchService.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Branch>())).Callback<int, Branch>((id, b) => updatedBranch = b).Returns(Task.FromResult(true));


            var result = await _userManagerService.CreateUserAndAccount("John", DateTime.Now, "test@email.com", "Address", 1, "password");


            Assert.IsTrue(result);

            // Check for User and Account IDs to match
            Assert.AreEqual(createdUser.AccountId, createdAccount.AccountId);
            Assert.AreEqual(createdAccount.UserId, createdUser.UserId);

            // Check if the number of active users in the branch increased by 1
            Assert.AreEqual(1, updatedBranch.NumberOfActiveUsers);
        }
    }
}
