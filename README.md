# EI_Task_Testing

The project includes a suite of unit tests that ensure [WinForms_Library](https://github.com/AlexCKHO/WinForms_Library) behaviour and reliability. The tests are written using NUnit and Moq, and used in-memory database for mocking dependencies.

### Test Files

1. **AccountServiceShould.cs**: Tests related to account services like login and registration.
2. **BookManagerServiceShould.cs**: Tests for book management features.
3. **LibraryRepositoryTests.cs**: Tests for CRUD operations in the library repository.
4. **UserManagerServiceShould.cs**: Tests related to user management.
5. **ValidationServiceShould.cs**: Tests for various validation services like password and email validation.

### Running the Tests

To run the tests, follow these steps:

1. Open the solution in Visual Studio.
2. Go to `Test` > `Run All Tests`.

You can also run individual tests by right-clicking on the test method and selecting `Run Test`.
