using CustomerProjectApi.Contracts.Messages;
using CustomerProjectApi.Domain;
using CustomerProjectApi.Mapping;
using CustomerProjectApi.Messaging;
using CustomerProjectApi.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace CustomerProjectApi.Services;

public class CustomerService : ICustomerService
{
    private readonly ISqsMessage _sqsMessage;
    private readonly IGitHubService _gitHubService;
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository,
        IGitHubService gitHubService,
        ISqsMessage sqsMessage)
    {
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
        _sqsMessage = sqsMessage;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var existingUser = await _customerRepository.GetAsync(customer.Id);
        if (existingUser is not null)
        {
            var message = $"A user with id {customer.Id} already exists";
            throw new ValidationException(message, GenerateValidationError(nameof(Customer), message));
        }

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        var customerDto = customer.ToCustomerDto();
        var response =  await _customerRepository.CreateAsync(customerDto);

        if (response)
        {
            await _sqsMessage.SendMessageAsync(customer.ToCustomerCreated());
        }

        return true;
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        var customerDto = await _customerRepository.GetAsync(id);
        return customerDto?.ToCustomer();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var customerDtos = await _customerRepository.GetAllAsync();
        return customerDtos.Select(x => x.ToCustomer());
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        var customerDto = customer.ToCustomerDto();

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        var response =  await _customerRepository.UpdateAsync(customerDto);
        if(response)
        {
            var message = customer.ToCustomerUpdated();
            await _sqsMessage.SendMessageAsync(message);
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _customerRepository.DeleteAsync(id);

        if (response)
        {
            await _sqsMessage.SendMessageAsync(new CustomerDeletedId { Id = id});
        }

        return true;
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new[]
        {
            new ValidationFailure(paramName, message)
        };
    }
}
