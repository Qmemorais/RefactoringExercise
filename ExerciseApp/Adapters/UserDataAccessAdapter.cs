using ExerciseApp.Entities;

namespace ExerciseApp.Adapters
{
    public class UserDataAccessAdapter : IUserDataAccessAdapter
    {
        public void AddUser(User user) => UserDataAccess.AddUser(user);
    }
}
