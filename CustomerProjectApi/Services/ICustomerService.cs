﻿using CustomerProjectApi.Domain;

namespace CustomerProjectApi.Services;

public interface ICustomerService
{
    Task<bool> CreateAsync(Customer customer);

    Task<Customer?> GetAsync(Guid id);

    Task<IEnumerable<Customer>> GetAllAsync();

    Task<bool> UpdateAsync(Customer customer);

    Task<bool> DeleteAsync(Guid id);
}
