using System;

namespace ExerciseApp
{
    public class UserService
    {
        private readonly CustomerRepository _customerRepository;
        public UserService()
        {
            _customerRepository = new CustomerRepository();
        }

        public bool AddUser(string firstname, string surname, string email, DateTime dateOfBirth, int customerId)
        {
            var user = CreateUser(firstname, surname, email, dateOfBirth, customerId);

            SetCreditLimit(user);

            if (!Validate(user)) return false;

            UserDataAccess.AddUser(user);

            return true;
        }

        private User CreateUser(string firstname, string surname, string email, DateTime dateOfBirth, int customerId)
            => new User { Firstname = firstname, Surname = surname, EmailAddress = email, DateOfBirth = dateOfBirth, Customer = _customerRepository.GetById(customerId) };

        private void SetCreditLimit(User user)
        {
            if (user.Customer.Name != "VeryImportantCustomer")
            {
                // Do credit check
                user.HasCreditLimit = true;

                using var userCreditService = new UserCreditServiceClient();

                var creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);

                if (user.Customer.Name == "ImportantCustomer")
                    creditLimit *= 2; //подвоїти кредитний ліміт

                user.CreditLimit = creditLimit;
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;

            return now.DayOfYear < dateOfBirth.DayOfYear
                ? age-- : age;
        }

        private bool Validate(User user)
        {
            if (string.IsNullOrEmpty(user.Firstname) || string.IsNullOrEmpty(user.Surname)
                || (user.EmailAddress.Contains("@") && !user.EmailAddress.Contains("."))
                || CalculateAge(user.DateOfBirth) < 21
                || user.HasCreditLimit && user.CreditLimit < 500)
                return false;

            return true;
        }
    }
}