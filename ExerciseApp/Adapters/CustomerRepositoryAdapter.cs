using ExerciseApp.Entities;

namespace ExerciseApp.Adapters
{
    public class CustomerRepositoryAdapter : ICustomerRepositoryAdapter
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerRepositoryAdapter() => _customerRepository = new CustomerRepository();

        public Customer GetById(int id) => _customerRepository.GetById(id);
    }
}