using ConsumerApi.Messages;
using MediatR;

namespace ConsumerApi.Handlers
{
    public class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
    {
        private readonly ILogger<CustomerCreatedHandler> _logger;
        public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(CustomerCreated request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{request.Name}");

            //throw new Exception("Something went wrong");

            await Task.CompletedTask;
        }
    }
}