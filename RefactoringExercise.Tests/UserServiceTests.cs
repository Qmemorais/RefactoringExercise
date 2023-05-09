using ExerciseApp;
using ExerciseApp.Adapters;
using ExerciseApp.Entities;
using Moq;
using NUnit.Framework;
using System;

namespace RefactoringExercise.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<ICustomerRepositoryAdapter> _customerRepository;
        private Mock<IUserDataAccessAdapter> _userAccessAdapter;
        private Mock<IUserCreditService> _userCreditServiceClient;

        private const int _customerId = 4;

        [SetUp]
        public void Setup()
        {
            _userCreditServiceClient = new Mock<IUserCreditService>();
            _customerRepository = new Mock<ICustomerRepositoryAdapter>();
            _userAccessAdapter = new Mock<IUserDataAccessAdapter>();
            _userService = new UserService(_customerRepository.Object, _userAccessAdapter.Object, _userCreditServiceClient.Object);
            //setup main customer
            SetupCustomer(_customerId, "VeryImportantCustomer");
        }

        [TestCase("Bill", "Carter", "Bill.carter@mail.com", "1983-1-1", _customerId), Description("Correct user and customer values. We check validation")]
        public void AddUser_CorrectVeryImportantUser_ReturnTrue(string firstname, string surname, string email, DateTime dateOfBirth, int customerId)
        {
            var result = _userService.AddUser(firstname, surname, email, dateOfBirth, customerId);
            Assert.IsTrue(result);
        }

        [TestCase("", "Carter", "Bill.carter@mail.com", "1983-1-1", _customerId), Description("Empty name or surname")]
        [TestCase("Bill", "", "Bill.carter@mail.com", "1983-1-1", _customerId)]
        [TestCase(null, "Carter", "Bill.carter@mail.com", "1983-1-1", _customerId)]
        public void AddUser_NotValidNameSurname_ReturnFalse(string firstname, string surname, string email, DateTime dateOfBirth, int customerId)
        {
            var result = _userService.AddUser(firstname, surname, email, dateOfBirth, customerId);
            Assert.IsFalse(result);
        }

        [TestCase("Bill", "Carter", "Bill.carter@mail.com", "2002-5-10", _customerId), Description("User age is one day before 21 or age is less then 21")]
        [TestCase("Bill", "Carter", "Bill.carter@mail.com", "2012-5-8", _customerId)]
        public void AddUser_AgeLessThen21_ReturnFalse(string firstname, string surname, string email, DateTime dateOfBirth, int customerId)
        {
            var result = _userService.AddUser(firstname, surname, email, dateOfBirth, customerId);
            Assert.IsFalse(result);
        }

        [TestCase("Bill", "Carter", "Billcarter@mailcom", "2000-5-8", _customerId), Description("User email have @ instead of '.'")]
        public void AddUser_InvalidEmailValue_ReturnFalse(string firstname, string surname, string email, DateTime dateOfBirth, int customerId)
        {
            var result = _userService.AddUser(firstname, surname, email, dateOfBirth, customerId);
            Assert.IsFalse(result);
        }

        [TestCase("Bill", "Carter", "Billcarter@mailcom", "2000-5-8", _customerId, "UsualCustomer"), 
            Description("User NOT VeryImportantCustomer and has credit limit")]
        [TestCase("Bill", "Carter", "Billcarter@mailcom", "2000-5-8", _customerId, "ImportantCustomer")]
        public void AddUser_UserHasIncorrectCreditLimit_ReturnFalse(string firstname, 
            string surname, string email, DateTime dateOfBirth, int customerId, string customerName)
        {
            SetupCustomer(customerId, customerName);
            SetupCreditLimit(firstname, surname, dateOfBirth);
            var result = _userService.AddUser(firstname, surname, email, dateOfBirth, customerId);
            Assert.IsFalse(result);
        }

        private void SetupCustomer(int customerId, string Name)
        {
            var customer = new Customer { Id = customerId, Name = Name, CustomerStatus = CustomerStatus.none };
            _customerRepository.Setup(x => x.GetById(customerId)).Returns(customer);
        }

        private void SetupCreditLimit(string firstname, string surname, DateTime dateOfBirth)
        {
            var creditLimit = 100;
            _userCreditServiceClient.Setup(x => x.GetCreditLimit(firstname, surname, dateOfBirth)).Returns(creditLimit);
        }
    }
}