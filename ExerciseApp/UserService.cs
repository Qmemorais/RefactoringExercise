using System;
using ExerciseApp.Adapters;
using ExerciseApp.Entities;

namespace ExerciseApp
{
    public class UserService
    {
        private readonly ICustomerRepositoryAdapter _customerRepository;
        private readonly IUserDataAccessAdapter _userDataAccess;
        private IUserCreditService _userCreditService;

        public UserService()
        {
            _customerRepository = new CustomerRepositoryAdapter();
            _userDataAccess = new UserDataAccessAdapter();
        }

        public UserService(ICustomerRepositoryAdapter customerRepository, 
            IUserDataAccessAdapter userDataAccess, 
            IUserCreditService userCreditService)
        {
            _customerRepository = customerRepository;
            _userDataAccess = userDataAccess;
            _userCreditService = userCreditService;
        }

        public bool AddUser(string firstname, string surname, string email, DateTime dateOfBirth, int customerId)
        {
            var user = CreateUser(firstname, surname, email, dateOfBirth, customerId);

            SetCreditLimit(user);

            if (Validate(user)) return false;

            _userDataAccess.AddUser(user);

            return true;
        }

        private User CreateUser(string firstname, string surname, string email, DateTime dateOfBirth, int customerId)
            => new() { Firstname = firstname, Surname = surname, EmailAddress = email, DateOfBirth = dateOfBirth, Customer = _customerRepository.GetById(customerId) };

        private void SetCreditLimit(User user)
        {
            if (user.Customer.Name != "VeryImportantCustomer")
            {
                // Do credit check
                user.HasCreditLimit = true;
                _userCreditService ??= new UserCreditServiceClient();
                var creditLimit = _userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);

                if (user.Customer.Name == "ImportantCustomer")
                    creditLimit *= 2; //подвоїти кредитний ліміт

                user.CreditLimit = creditLimit;
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;

            return now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)
                ? age - 1 : age;
        }

        private bool Validate(User user)
            => (string.IsNullOrEmpty(user.Firstname) || string.IsNullOrEmpty(user.Surname)
                || (user.EmailAddress.Contains("@") && !user.EmailAddress.Contains("."))
                || CalculateAge(user.DateOfBirth) < 21
                || (user.HasCreditLimit && user.CreditLimit < 500));
    }
}