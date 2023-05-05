using System;

namespace ExerciseApp
{
    public class User
    {
        public Customer Customer { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public bool HasCreditLimit { get; set; }
        public int CreditLimit { get; set; }
    }
}