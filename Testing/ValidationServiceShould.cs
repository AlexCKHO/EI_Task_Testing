using EI_Task.Services;
using System.Windows.Forms;
using Moq;

namespace Testing
{
    public class ValidationServiceShould
    {
        private IValidationService _validationService;
        private Mock<Control.ControlCollection> _mockControls;
        private Mock<ErrorProvider> _mockErrorProvider;


        [SetUp]
        public void SetUp()
        {
            _validationService = new ValidationService();
        }

        [Test]
        public void ValidatePublishedYear_ValidYear_ReturnsTrue()
        {
            Assert.IsTrue(_validationService.ValidatePublishedYear(2021));
        }

        [Test]
        public void ValidatePublishedYear_InvalidYear_ReturnsFalse()
        {
            Assert.IsFalse(_validationService.ValidatePublishedYear(0));
        }

        [Test]
        public void ValidatingPasswordInput_ValidPassword_ReturnsTrue()
        {
            Assert.IsTrue(_validationService.ValidatingPasswordInput("Test12@"));
        }

        [Test]
        public void ValidatingPasswordInput_InvalidPassword_ReturnsFalse()
        {
            Assert.IsFalse(_validationService.ValidatingPasswordInput("Test"));
        }

        [Test]
        public void ValidatingEmailInput_ValidEmail_ReturnsTrue()
        {
            Assert.IsTrue(_validationService.ValidatingEmailInput("test@test.com"));
        }

        [Test]
        public void ValidatingEmailInput_InvalidEmail_ReturnsFalse()
        {
            Assert.IsFalse(_validationService.ValidatingEmailInput("testtest.com"));
        }

        [Test]
        public void IsValidDate_ValidDate_ReturnsTrue()
        {
            Assert.IsTrue(_validationService.IsValidDate(15, 8, 2023));
        }

        [Test]
        public void IsValidDate_InvalidDate_ReturnsFalse()
        {
            Assert.IsFalse(_validationService.IsValidDate(31, 2, 2023));
        }

        [Test]
        public void ValidateCellValue_ValidName_ReturnsTrue()
        {
            Assert.IsTrue(_validationService.ValidateCellValue("Name", "ValidName"));
        }

        [Test]
        public void ValidateCellValue_InvalidName_ReturnsFalse()
        {
            Assert.IsFalse(_validationService.ValidateCellValue("Name", ""));
        }

        [Test]
        public void ValidateCellValue_ValidPublishedYear_ReturnsTrue()
        {
            Assert.IsTrue(_validationService.ValidateCellValue("PublishedYear", 2021));
        }

        [Test]
        public void ValidateCellValue_InvalidPublishedYear_ReturnsFalse()
        {
            Assert.IsFalse(_validationService.ValidateCellValue("PublishedYear", 0));

        }


        
    }
}
