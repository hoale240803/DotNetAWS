using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using CustomerApi.Contracts.Data;
using System.Text.Json;
using Amazon.DynamoDBv2.DocumentModel;

namespace CustomerApi.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _amazonDynamoDB;

    private readonly string _customerTable = "customers";

    public CustomerRepository(IAmazonDynamoDB amazonDynamoDB)
    {
        _amazonDynamoDB = amazonDynamoDB;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdateAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        var item = new PutItemRequest
        {
            TableName = _customerTable,
            Item = customerAsAttributes
        };
        var res = await _amazonDynamoDB.PutItemAsync(item);

        return res.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = _customerTable,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue{ S = id.ToString()} },
                { "sk", new AttributeValue{ S = id.ToString()} }
            }
        };

        var result = await _amazonDynamoDB.GetItemAsync(getItemRequest);
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        customer.UpdateAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        var item = new PutItemRequest
        {
            TableName = _customerTable,
            Item = customerAsAttributes
        };
        var res = await _amazonDynamoDB.PutItemAsync(item);

        return res.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}