using ExerciseApp.Entities;

namespace ExerciseApp.Adapters
{
    public interface ICustomerRepositoryAdapter
    {
        Customer GetById(int id);
    }
}
