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

        public bool AddUser(string firеnam, string surname, string email, DateTime dateOfBirth, int customerId)
        {
            if (string.IsNullOrEmpty(firеnam) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            if (CalculateAge(dateOfBirth) < 21)
            {
                return false;
            }

            var user = new User
            {
                Customer = _customerRepository.GetById(customerId),
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firеnam,
                Surname = surname
            };

            SetCreditLimit(user);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            
            UserDataAccess.AddUser(user);

            return true;
        }

        private void SetCreditLimit(User user)
        {
            if (user.Customer.Name == "VeryImportantCustomer")
            {
                // Перевірити кредитний ліміт
                user.HasCreditLimit = false;
            }
            else
            {
                // Do credit check
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditServiceClient())
                {
                    var creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);

                    if (user.Customer.Name == "ImportantCustomer")
                        creditLimit = creditLimit * 2; //подвоїти кредитний ліміт

                    user.CreditLimit = creditLimit;
                }
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age;
        }
    }
}