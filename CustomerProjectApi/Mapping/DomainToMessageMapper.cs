using CustomerProjectApi.Contracts.Messages;
using CustomerProjectApi.Domain;

namespace CustomerProjectApi.Mapping
{
    public static class DomainToMessageMapper
    {
        public static CustomerCreated ToCustomerCreated(this Customer customer)
        {
            return new CustomerCreated
            {
                Id = customer.Id,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth,
                GithubUsername = customer.GitHubUsername,
                Name = customer.FullName
            };
        }

        public static CustomerUpdated ToCustomerUpdated(this Customer customer)
        {
            return new CustomerUpdated
            {
                Id = customer.Id,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth,
                GithubUsername = customer.GitHubUsername,
                Name = customer.FullName
            };
        }
    }
}