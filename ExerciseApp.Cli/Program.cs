using System;

namespace ExerciseApp.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            AddUser(args);
        }

        public static void AddUser(string[] args)
        {
            // ВЗАГАЛІ НЕ ЗМІНЮЙТЕ ЦЕЙ ФАЙЛ

            var userService = new UserService();
            var result = userService.AddUser("Bill", "Carter", "Bill.carter@mail.com", new DateTime(1983, 1, 1), 4);
            Console.WriteLine("Adding Bill Carter was " + (result ? "successful" : "unsuccessful"));
        }
    }
}