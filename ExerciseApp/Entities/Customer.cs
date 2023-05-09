namespace ExerciseApp.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CustomerStatus CustomerStatus { get; set; }
    }
}